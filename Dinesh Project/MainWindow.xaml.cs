﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using log4net;

namespace Dinesh_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<TabItem> listofTabls=new List<TabItem>();
        public MainWindow()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
            LoginPopup window = new LoginPopup();
            bool? val=window.ShowDialog();
            loginName.Text = window.LoginId;
        }

        private void Page1_Click(object sender, RoutedEventArgs e)
        {
            const string tabName = "Transactions";
            
            if (!TabExists(tabName))
            {
                TabItem tabitem = new TabItem();
                tabitem.Header = tabName ;
                Frame tabFrame = new Frame();
                Transactions page1 = new Transactions();
                tabFrame.Content = page1;
                tabitem.Content = tabFrame;
                tabitem.Name =tabName;
                tabControlView.Items.Add(tabitem);
                tabControlView.SelectedItem = tabitem;
            }
            else
            {
                List<TabItem> tabitem = (from TabItem item in tabControlView.Items
                                   where item.Name.Equals(tabName)
                                   select item).ToList();
                if (tabitem.Any())
                {
                    tabControlView.SelectedItem = tabitem.First();
                }
                
            }
                //MessageBox.Show("Tab is Already Open or Too Many Tabs", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void Page2_Click(object sender, RoutedEventArgs e)
        {

            const string tabName = "Reports";
            if (!TabExists(tabName))
            {
                TabItem tabitem = new TabItem();
                tabitem.Header = tabName;
                Frame tabFrame = new Frame();
                Reports page1 = new Reports();
                tabFrame.Content = page1;
                tabitem.Content = tabFrame;
                tabitem.Name = tabName;
                tabControlView.Items.Add(tabitem);
                tabControlView.SelectedItem = tabitem;
            }
            else
            {
                List<TabItem> tabitem = (from TabItem item in tabControlView.Items
                                   where item.Name.Equals(tabName)
                                   select item).ToList();
                if (tabitem.Any())
                {
                    tabControlView.SelectedItem = tabitem.First();
                }
                
            }
                //MessageBox.Show("Tab is Already Open or Too Many Tabs", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        public bool TabExists(string Name)
        {
            var activeTabs = (from TabItem item in tabControlView.Items
                              select item.Name);
            if (activeTabs.Contains(Name))
                return true;
            else
                return false;
        }

        private void Page3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TabControlClicked(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void LoginIdClicked(object sender, RoutedEventArgs e)
        {
            if (!drpDownLogin.Visibility.Equals(Visibility.Visible))
                drpDownLogin.Visibility = Visibility.Visible;
            else
                drpDownLogin.Visibility = Visibility.Hidden;

        }

        private void ChangePassword(object sender, RoutedEventArgs e)
        {
            LoginPopup popup = new LoginPopup(true);
            var passed= popup.ShowDialog();
            if (!string.IsNullOrEmpty(popup.Password))
            {
                CoreOperations.EditPassword(loginName.Text, popup.Password);
            }

        }

        private void lostfocus(object sender, RoutedEventArgs e)
        {
            drpDownLogin.Visibility = Visibility.Hidden;
        }
    }
}
