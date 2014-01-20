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
        private int startIdCust = 0,NumberofItemsCust=5;
        public Reports()
        {
            InitializeComponent();
            BindDataofTechnicians();
            BindDataofCustomers();
            BindDataofVehicles();
        }

        public void BindDataofTechnicians()
        {
            List<Technician> techList = CoreOperations.GetAllTechnicians(String.Empty,string.Empty);
            techList = CoreOperations.RemoveAdditionalData(techList, startIdCust, NumberofItemsCust);
            grdTechData.ItemsSource = techList;
            
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
            List<Technician> techList = CoreOperations.GetAllTechnicians(enteredText,enteredID);
            grdTechData.ItemsSource = techList;
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

        #region Customers
        private void BindDataofCustomers()
        {
            List<CustomerData> customerData=CoreOperations.GetCustomizedCustomerdData();
            grdCustData.ItemsSource = customerData;
            matchCustSearchRequest();
                
        }

        private void txtcustRegistrationId_TextChanged(object sender, TextChangedEventArgs e)
        {
            matchCustSearchRequest();
            startIdCust = 0;
        }

        public void matchCustSearchRequest()
        {
            try
            {
                string enteredText = txtCustomerName.Text;
                string enteredPhone = txtCustomerPhone.Text;
                List<Customer> custList = CoreOperations.GetAllOwnersByName(enteredText, enteredPhone);
                var customizedlist = CoreOperations.GetCustomizedCustomerdData(custList);
                var finalizedList = customizedlist.ToList();
                finalizedList = CoreOperations.RemoveAdditionalData(finalizedList, startIdCust, NumberofItemsCust);
                grdCustData.ItemsSource = finalizedList;
            }
            catch(Exception ex)
            {
                Utility.WriteLogError("Exception OCcurred in matchCustSearchRequest\n" + ex.ToString());
            }
        }

        private void txtcustPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            matchCustSearchRequest();
        }

        private void FirstCustCliked(object sender, RoutedEventArgs e)
        {
            startIdCust = 0;
            matchCustSearchRequest();
        }

        private void PrevCustClicked(object sender, RoutedEventArgs e)
        {
            if (startIdCust <= NumberofItemsCust)
                startIdCust = 0;
            else
                startIdCust -= NumberofItemsCust;
        }

        private void nextcustClicked(object sender, RoutedEventArgs e)
        {
            startIdCust += NumberofItemsCust;
                matchCustSearchRequest();
        }

        private void lastCustClicked(object sender, RoutedEventArgs e)
        {
            startIdCust = -NumberofItemsCust;
        }

       

        private void grdCustData_RowEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            CustomerData row = (CustomerData)dg.SelectedItems[0];
            int key = (int)row.CustomerID;
            string Name = row.Name;
            string RegID = row.RegistrationID;
            string phone = row.Phone;
            string address = row.Address;
            if (!string.IsNullOrEmpty(Name))
            {
                bool doesExist = (CoreOperations.doesOwnerExists(key) <= -1) ? false : true;
                if (doesExist)
                {
                    if (CoreOperations.EditACustomer(key, RegID,Name,phone,address))
                        MessageBox.Show("Edit is Successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Edit is UnSuccessful", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (CoreOperations.AddANewCustomer(Name, Utility.CreateRandomID(Name),phone,address))
                        MessageBox.Show("Addition is Successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Addition is UnSuccessful", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                }   
            }
            matchCustSearchRequest();
        }

        #endregion

        #region Vehicles

        private void BindDataofVehicles()
        {
            List<Vehicle> customerData = CoreOperations.GetAllVehicles();
            grdVehicData.ItemsSource = customerData.ToList();

        }

        public void matchVehicleList()
        {

        }
        private void txtVechicID_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void txtOwner_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void grdVehicData_RowEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            Vehicle row = (Vehicle)dg.SelectedItems[0];
            int key = (int)row.VehicleID;
            string RegID = row.RegistrationNumber;
            string CustomerName = row.Customer.Name;
            string vehicleType=row.VehicleType;
            if (!string.IsNullOrEmpty(Name))
            {
                bool doesExist = (CoreOperations.doesVehicleExist(RegID) <= -1) ? false : true;
                if (doesExist)
                {
                    if (CoreOperations.EditAVehicle(key, RegID, vehicleType, (int)row.Customer.CustomerID))
                        MessageBox.Show("Edit is Successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Edit is UnSuccessful", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (CoreOperations.AddANewVehicle(RegID,vehicleType,(int)row.Customer.CustomerID))
                        MessageBox.Show("Addition is Successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Addition is UnSuccessful", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            matchCustSearchRequest();
        }
        #endregion

        




    }
}
