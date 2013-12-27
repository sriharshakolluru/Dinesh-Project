using System;
using System.Collections.Generic;
using System.Collections;
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
            BindDataofTechnicians();
            BindDataofCustomers();
        }

        public void BindDataofTechnicians()
        {
            DataTable techList = CoreOperations.GetAllTechnicians(String.Empty,string.Empty);
            grdTechData.ItemsSource = techList.AsDataView();
            techList.Dispose();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }


        #region Technicians
        private void grdTechData_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            DataGrid dg= (DataGrid)sender;
            DataRowView row = (DataRowView)dg.SelectedItems[0];
            string key = row["Id"].ToString();
            string Name = row["Name"].ToString();
            string RegID = row["RegistrationID"].ToString();
            if (!string.IsNullOrEmpty(Name))
            {
                bool doesExist = (CoreOperations.doesTechnicianExists(Name) <= -1) ? false : true;
                if (doesExist)
                {
                    if (CoreOperations.EditaTechnician(int.Parse(key), Name, RegID))
                        MessageBox.Show("Edit is Successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Edit is UnSuccessful", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (CoreOperations.AddANewTechnician(Name, Utility.CreateRandomID(Name)))
                        MessageBox.Show("Addition is Successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Addition is UnSuccessful", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            matchSearchRequest();
        }

        private void txtTechName_TextChanged(object sender, TextChangedEventArgs e)
        {
            matchSearchRequest();
        }

        
        public void matchSearchRequest()
        {
            string enteredText = txtTechName.Text;
            string enteredID = txttechRegistrationId.Text;
            DataTable techList = CoreOperations.GetAllTechnicians(enteredText,enteredID);
            grdTechData.ItemsSource = techList.AsDataView();
            techList.Dispose();
        }

        private void txttechRegistrationId_TextChanged(object sender, TextChangedEventArgs e)
        {
            matchSearchRequest();
        }

        private void delButton_Click(object sender, RoutedEventArgs e)
        {
            int index = grdTechData.SelectedIndex;
            var selectedItems = grdTechData.SelectedItems;
            bool delOk=false;
            StringBuilder FailedIDs = new StringBuilder("The Technicians which are failed to be deleted are : ");
            foreach (DataRowView selectedItem in selectedItems)
            {
                delOk = CoreOperations.DeleteTechnician(selectedItem[2].ToString());
                if (!delOk)
                {
                    FailedIDs.Append(selectedItem[1]);
                    continue;
                }
            }
            if (delOk)
                matchSearchRequest();
            else
            {
                MessageBox.Show(FailedIDs.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        #endregion

        
        private void BindDataofCustomers()
        {
            CustomersData[] datasource = new CustomersData[1];
            
            CustomersData data = new CustomersData();
            datasource[0] = data;
            data.ID = "asdasdf";
            data.Name = "sample";
            
            ArrayList vechicleLIst=new ArrayList();
            vechicleLIst.Add("abc");
            vechicleLIst.Add("abc2");
            ListCollectionView list=new ListCollectionView(vechicleLIst);
            
            //data.log= list;
            grdCustData.DataContext = datasource;
            grdCustData.ItemsSource = datasource;

            
            

                
        }

        



    }
}
