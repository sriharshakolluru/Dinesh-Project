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
using DataOperations;
using System.Data;
namespace Dinesh_Project
{
    /// <summary>
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : Page
    {
        public Reports()
        {
            InitializeComponent();
            BindData();
        }

        public void BindData()
        {
            DataTable techList = CoreOperations.GetAllTechnicians(String.Empty);
            grdTechData.ItemsSource = techList.AsDataView();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void grdTechData_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            DataGrid dg= (DataGrid)sender;
            DataRowView row = (DataRowView)dg.SelectedItems[0];
            string str = row["Name"].ToString();
        }
    }
}
