   M S B A M L   `   `   `  ����D  @RealHosts, Version=4.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:RealHosts	RealHosts  5      =#clr-namespace:RealHosts.Views.PagesRealHosts.Views.Pages  5          RealHosts.MainWindow   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      )localclr-namespace:RealHosts     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      5pages#clr-namespace:RealHosts.Views.Pages     5      F BWpf.Ui.Tray, Version=4.0.0.0, Culture=neutral, PublicKeyToken=null6tray+http://schemas.lepo.co/wpfui/2022/xaml/tray  5	      A =Wpf.Ui, Version=4.0.0.0, Culture=neutral, PublicKeyToken=null/ui&http://schemas.lepo.co/wpfui/2022/xaml  5
        � Title$  	RealHosts��5      $
��1100��5      $	��650��5         ApplicationBackgroundBrush  Wpf.Ui.Markup.Design   
Background# �   5        TextFillColorPrimaryBrush   
Foreground# �  5      "  Wpf.Ui.Controls.FluentWindow!   ExtendsContentIntoTitleBar$
 True��5      #���  5         WindowBackdropType$
 Mica=�5         WindowCornerPreference$ Round=�5       � WindowStartupLocation$ CenterScreen=�5       J� InputBindings 5      �� 5   
   
 �� Key$ F��5      	 �� 	Modifiers$	 Controlf�5      
 �� Command
 5      ��  �� ElementName$ AutoSuggestBox�� �� Path$ FocusCommand�5      .��5       �  � RowDefinitions 5!   
   �� 5"      $
��Auto �6   �� 5#      $��* �6   .+�5&   
   $  Wpf.Ui.Controls.NavigationView  -   5          Name$ RootNavigation��5&      $��1��64      AutoSuggestBox 5'      $  Wpf.Ui.Controls.AutoSuggestBox  5(      -   5          Name$ AutoSuggestBox��5(   $      PlaceholderText$ Search��6<      Icon 5)      '  !Wpf.Ui.Controls.IconSourceElement  5*         
IconSource. 5+      &   Wpf.Ui.Controls.SymbolIconSource     Symbol$ Search24=�62   5,      5.         Header 50      #  Wpf.Ui.Controls.BreadcrumbBar  51      $��	42,32,0,0q�52      $��28(�53      $��DemiBold"�54         	MenuItems 56      (  "Wpf.Ui.Controls.NavigationViewItem  57      $��Hosts��58         NavigationCacheMode$ Enabled=�59         TargetPageType)	   #RealHosts.Views.Pages.DashboardPage" 	 5:         Icon 5;       
  Wpf.Ui.Controls.SymbolIcon
  5<       
  Symbol$ Home24=�6(   5>        5?      $
��Ping��5@      $ Enabled=�5A      $   RealHosts.Views.Pages.PingPage"  5B       5C      
  5D      $ NetworkCheck20=�6(   5F         FooterMenuItems 5H        5I      $��Settings��5J      $ Disabled=�5K      (   "RealHosts.Views.Pages.SettingsPage"  5L       5M      
  5N      $ 
Settings24=�6(   5P      5R        Wpf.Ui.Controls.TitleBar  5T   
      Title$ 	RealHosts��5U      $��0��5V         Icon*  $Wpf.Ui.Controls.IconElementConverter$; 5pack://application:,,,/Assets/applicationIcon-256.png 5W      %  Wpf.Ui.Tray.Controls.NotifyIcon  5Y   
   $��0��5Z         FocusOnLeftClick$
 True��5[         Icon$; 5pack://application:,,,/Assets/applicationIcon-256.png��5\          MenuOnRightClick$
  True��5]      !   TooltipText$! 	RealHosts��5^      "   Menu" 5_      �� 5`      ��6   �� *ViewModel.TrayMenuItems+# �� Mode$# OneWay=�5b      5c      5d      