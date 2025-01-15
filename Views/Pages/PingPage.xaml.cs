using HtmlAgilityPack;
using RealHosts.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RealHosts.Views.Pages;

/// <summary>
/// </summary>
public partial class PingPage : Page
{
    const string wwwFileName = "www.data";
    const string hostStartTag = "###### start tim github hosts AT";
    const string hostEndTag = "###### end tim github hosts ######";

    public ObservableCollection<DataDomains> DomainList { get; set; }

    public PingPage()
    {
        InitializeComponent();
        DomainList = new ObservableCollection<DataDomains>();
        LoadDomainListFromFile();
        myDataGrid.ItemsSource = DomainList;
    }

    private async void Button_Click_1(object sender, RoutedEventArgs e)
    {
        testBtn.Content = "Testing";
        testBtn.IsEnabled = false;
        myProgress.Maximum = myDataGrid.Items.Count;
        myProgress.Minimum = 0;
        myProgress.UpdateLayout();
        await GetIpAddressesAsync();
        save2Hosts(myDataGrid);
        testBtn.Content = "Test";
        testBtn.IsEnabled = true;
    }

    private async Task GetIpAddressesAsync()
    {
        for (int i = 0; i < DomainList.Count; i++)
        {
            DataDomains dm = await GetDomeinIpsAsync(DomainList[i]);
            UpdateDomainInList(dm);
            myProgress.Value = i + 1;
        }
    }

    private void UpdateDomainInList(DataDomains dm)
    {
        if (dm.Ttl != 0 && dm.Ip != "")
        {
            for (int i = 0; i < DomainList.Count; i++)
            {
                if (DomainList[i].Domain == dm.Domain)
                {
                    DomainList[i] = dm;
                    break;
                }
            }
        }
    }

    private async Task<DataDomains> GetDomeinIpsAsync(DataDomains domain)
    {
        using HttpClient client = new HttpClient();
        try
        {
            HttpResponseMessage response = await client.GetAsync($"https://www.ipaddress.com/website/{domain.Domain}");
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                string subContent = responseContent.Split("Subdomains for GitHub.com")[0];
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(subContent);
                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//a");
                if (nodes != null)
                {
                    List<string> ipAddresses = new List<string>();
                    foreach (HtmlNode node in nodes)
                    {
                        string href = node.GetAttributeValue("href", "");
                        if (href.StartsWith("https://www.ipaddress.com/ipv4/"))
                        {
                            ipAddresses.Add(node.InnerText);
                        }
                    }
                    long minTtl = 9999;
                    string ip = ipAddresses.Count > 0 ? ipAddresses[0] : "";
                    foreach (var ip_item in ipAddresses)
                    {
                        long ttl = await pingIpTtl(ip_item);
                        if (minTtl > ttl)
                        {
                            ip = ip_item;
                            minTtl = ttl;
                        }
                    }
                    if (minTtl != 9999)
                    {
                        domain.Ip = ip;
                        domain.Ttl = minTtl;
                        return domain;
                    }
                }
            }
            else
            {
                //Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        catch (HttpRequestException ex)
        {
            //Console.WriteLine($"Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            //Console.WriteLine($"Error: {ex.Message}");
        }
        return new DataDomains { };
    }

    private async Task<long> pingIpTtl(string ipAddress)
    {
        int timeout = 1000;
        int attempts = 4;
        long minSec = 9999;
        using Ping pingSender = new Ping();
        for (int i = 0; i < attempts; i++)
        {
            try
            {
                PingReply reply = await pingSender.SendPingAsync(ipAddress, timeout);
                if (reply.Status == IPStatus.Success)
                {
                   // Console.WriteLine($"Ping Success，TTL: {reply.Options.Ttl}, Time: {reply.RoundtripTime} ms");
                    if (minSec > reply.RoundtripTime)
                    {
                        minSec = reply.RoundtripTime;
                    }
                }
                else
                {
                   // Console.WriteLine($"Ping Fail，Status: {reply.Status}");
                }
            }
            catch (PingException e)
            {
                //Console.WriteLine($"Ping Error: {e.Message}");
            }
        }
        return minSec;
    }


    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(addInput.Text))
        {
            DomainList.Add(new DataDomains { Domain = addInput.Text, Ip = "", Ttl = 0 });
            SaveDomainListToFile();
            addInput.Text = "";
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        DataGridRow row = DataGridRow.GetRowContainingElement(button);
        DataDomains domainItem = (DataDomains)row.Item;
        DomainList.Remove(domainItem);
        SaveDomainListToFile();
    }


    private void LoadDomainListFromFile()
    {
        List<string> wwwList = ReadFileToList(wwwFileName);
        if (wwwList.Count == 0)
        {
            wwwList.Add("www.github.com");
        }
        foreach (string www in wwwList)
        {
            if (www != null && !www.StartsWith("#"))
            {
                DomainList.Add(new DataDomains { Domain = www, Ip = "", Ttl = 0 });
            }
        }
    }


    private void SaveDomainListToFile()
    {
        List<string> wwwList = DomainList.Select(d => d.Domain).ToList();
        SaveListToFile(wwwList, wwwFileName);
    }


    static List<string> ReadFileToList(string filePath)
    {
        List<string> lines = new List<string>();
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
        }
        catch (FileNotFoundException)
        {
           // Console.WriteLine($"File {filePath} Not Found。");
        }
        catch (Exception ex)
        {
           // Console.WriteLine($"Read File Error: {ex.Message}");
        }
        return lines;
    }


    static void SaveListToFile(List<string> lines, string filePath)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (string line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }
        catch (Exception ex)
        {
           // Console.WriteLine($"Save File Error: {ex.Message}");
        }
    }


    static string RemoveContent(string input, string st, string et)
    {
        int startIndex = input.IndexOf(st);
        int endIndex = input.LastIndexOf(et) + et.Length;
        if (startIndex == -1 || endIndex == -1 || startIndex >= endIndex)
        {
            return input;
        }
        string before = input.Substring(0, startIndex);
        string after = input.Substring(endIndex);
        string result = before + after;
        result = result.Replace("\r\n\r\n", "\r\n").Trim();
        return result;
    }


    static void save2Hosts(DataGrid dg)
    {
        string hostsFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts");
        try
        {
            string hostsContent = File.ReadAllText(hostsFilePath);
            hostsContent = RemoveContent(hostsContent, hostStartTag, hostEndTag);
            try
            {
                string st = hostStartTag + " " + DateTime.Now.ToString("G") + " ######";
                using (StreamWriter writer = new StreamWriter(hostsFilePath, false))
                {
                    writer.WriteLine(hostsContent);
                    writer.WriteLine(st);
                    for (int i = 0; i < dg.Items.Count; i++)
                    {
                        DataGridRow row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(i);
                        DataDomains domain = (DataDomains)row.Item;
                        if (domain.Ip != "" && domain.Ttl != 0)
                        {
                            writer.WriteLine(domain.Ip + "    " + domain.Domain);
                        }
                    }
                    writer.WriteLine(hostEndTag);
                }
                MessageBox.Show("Result Saved Into Hosts.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Saved Into Hosts.：{ex.Message}");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error Saved Into Hosts.：{ex.Message}");
        }
    }
}