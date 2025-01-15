// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using Wpf.Ui.Controls;
using MessageBox = Wpf.Ui.Controls.MessageBox;

namespace RealHosts.Views.Pages;

/// <summary>
/// Interaction logic for DashboardPage.xaml
/// </summary>
public partial class DashboardPage
{
    private int _counter = 0;

    public DashboardPage()
    {
        DataContext = this;
        InitializeComponent();
        this.Loaded += Page_Loaded;


    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        this.readHosts();
    }

    private void readHosts()
    {
        try
        {
            string hostsFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts");
            string hostsContent = File.ReadAllText(hostsFilePath);
            hostsText.Text = hostsContent;

        }
        catch (Exception ex)
        {

            MessageBox messageBox = new MessageBox()
            {
                Title = "Msg",
                Content = "Reading Hosts Error:\n" + ex.ToString
            };
            messageBox.Show();
        }
    }


}
