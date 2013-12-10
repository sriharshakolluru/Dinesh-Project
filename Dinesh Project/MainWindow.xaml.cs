using System;
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
using System.Linq;

namespace Dinesh_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Page1_Click(object sender, RoutedEventArgs e)
        {
            TabItem tabitem = new TabItem();
            tabitem.Header = "Transactions";
            Frame tabFrame = new Frame();
            Transactions page1 = new Transactions();
            tabFrame.Content = page1;
            tabitem.Content = tabFrame;
            tabitem.Name = "Transactions";
            if (AllowTab(tabitem.Name))
                tabControlView.Items.Add(tabitem);
            else
                MessageBox.Show("Tab is Already Open or Too Many Tabs", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void Page2_Click(object sender, RoutedEventArgs e)
        {

            TabItem tabitem = new TabItem();
            tabitem.Header = "Reports";
            Frame tabFrame = new Frame();
            Reports page1 = new Reports();
            tabFrame.Content = page1;
            tabitem.Content = tabFrame;
            tabitem.Name = "Reports";
            if (AllowTab(tabitem.Name))
                tabControlView.Items.Add(tabitem);
            else
                MessageBox.Show("Tab is Already Open or Too Many Tabs", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        public bool AllowTab(string Name)
        {
            var activeTabs = (from TabItem item in tabControlView.Items
                              select item.Name);
            if (activeTabs.Contains(Name))
                return false;
            else if (activeTabs.Count() > 5)
                return false;
            else
                return true;
        }

        private void Page3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TabControlClicked(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
