using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private int startIdVehic = 0, NumberofItemsVehic = 5;
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
            techList = techList.Skip(startIdCust).Take(NumberofItemsCust).ToList();
            grdTechData.ItemsSource = techList;
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }


        #region Technicians
        private void grdTechData_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Utility.WriteLog("Entered Row Editing/Adding For Technicians");
            DataGrid dg= (DataGrid)sender;
            Technician row = (Technician)dg.SelectedItems[0];
            int key = row.Id;
            string Name = row.Name;
            string RegID = row.RegistrationID;
            if (!string.IsNullOrEmpty(Name))
            {
                bool doesExist = (CoreOperations.doesTechnicianExists(key) <= -1) ? false : true;
                if (doesExist)
                {
                    Utility.WriteLog(string.Format("Started Editing Name {0} with ID {1}", Name, key));
                    if (CoreOperations.EditaTechnician(key, Name, RegID))
                        MessageBox.Show("Edit is Successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Edit is UnSuccessful", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);

                    Utility.WriteLog(string.Format("Completed Editing Name {0} with ID {1}",Name,key));
                }
                else
                {
                    Utility.WriteLog(string.Format("Started Adding Name {0} ", Name));
                    if (CoreOperations.AddANewTechnician(Name, Utility.CreateRandomID(Name)))
                        MessageBox.Show("Addition is Successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Addition is UnSuccessful", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);

                    Utility.WriteLog(string.Format("Completed adding Name {0} ", Name));
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
            foreach (Technician selectedItem in selectedItems)
            {
                delOk = CoreOperations.DeleteTechnician(selectedItem.RegistrationID);
                if (!delOk)
                {
                    FailedIDs.Append(selectedItem.Name);
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
                if (startIdCust < 0)
                    startIdCust = finalizedList.Count - NumberofItemsCust-1;

                if(startIdVehic<=finalizedList.Count)
                finalizedList = finalizedList.Skip(startIdCust).Take(NumberofItemsCust).ToList();

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
            startIdCust -= NumberofItemsCust;
            if (startIdCust < 0)
                startIdCust = 0;
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
                    if (CoreOperations.AddANewCustomer(Name, Utility.CreateRandomID(Name),phone,address)>-1)
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
            matchVehicleList();
        }

        public void matchVehicleList()
        {
            try
            {
                string enteredText = txtvehicleID.Text;
                string enteredPhone = txtOwner.Text;
                List<Vehicle> vehcList = CoreOperations.GetVehiclesByRegistration(enteredText);
                vehcList = vehcList.Where(v => v.Customer.Phone.Contains(enteredPhone)).ToList();

                if (startIdVehic < 0)
                    startIdVehic = vehcList.Count - NumberofItemsVehic - 1;

                if (startIdVehic <= vehcList.Count)
                    vehcList = vehcList.Skip(startIdVehic).Take(NumberofItemsVehic).ToList();

                grdVehicData.ItemsSource = vehcList;
            }
            catch (Exception ex)
            {
                Utility.WriteLogError("Exception OCcurred in matchCustSearchRequest\n" + ex.ToString());
            }

        }
        private void txtVechicID_TextChanged(object sender, TextChangedEventArgs e)
        {
            startIdVehic = 0;
            matchVehicleList();
        }
        private void txtOwner_TextChanged(object sender, TextChangedEventArgs e)
        {
            startIdVehic = 0;
            matchVehicleList();
        }
        private void grdVehicData_RowEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Utility.WriteLog(string.Format("Entered Grid Vehicle Data Editing "));
            DataGrid dg = (DataGrid)sender;
            Vehicle row = (Vehicle)dg.SelectedItems[0];
            int key = (int)row.VehicleID;
            string RegID = row.RegistrationNumber;
            string CustomerName = row.Customer.Name;
            string vehicleType=row.VehicleType;
            
            if (!string.IsNullOrEmpty(RegID))
            {
                bool doesExist = (CoreOperations.doesVehicleExist(key ) <= -1) ? false : true;
                int ownerId = CoreOperations.doesOwnerExists(CustomerName);
                Utility.WriteLog(string.Format("In Vehic  Editing VehicleExists? : {0} User Exists? : {1}", doesExist, ownerId));
                if (doesExist)
                {
                    if (ownerId < 0)
                    {
                        if (CoreOperations.AddANewCustomer(CustomerName, Utility.CreateRandomID(CustomerName), string.Empty, string.Empty) < 0)
                        {
                            MessageBox.Show("Edit is UnSuccessful", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                            Utility.WriteLogError("Could not create the new user. Before Editing Vehicle");
                            return;
                        }
                        else
                        {
                            ownerId = CoreOperations.doesOwnerExists(CustomerName);
                            Utility.WriteLog(string.Format("In Vehic  Editing : Customer Edited Successfully. ID Created : {0}, name is {1} ", ownerId, CustomerName));
                        }
                    }

                    if (CoreOperations.EditAVehicle(key, RegID, vehicleType, ownerId))
                        MessageBox.Show("Edit is Successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Edit is UnSuccessful", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);

                    Utility.WriteLog(string.Format("Completed Editing Vehicle For the ID {0} : Name {1}", key, RegID));
                }
                else
                {
                    if (CoreOperations.AddANewVehicle(RegID,vehicleType,ownerId))
                        MessageBox.Show("Addition is Successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Addition is UnSuccessful", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                    Utility.WriteLog(string.Format("Completed Adding  Vehicle For the ID {0} : Owner {1}", RegID,ownerId));
                }
            }
            matchVehicleList();
        }
        #endregion

        private void FirstvehicCliked(object sender, RoutedEventArgs e)
        {
            startIdVehic = 0;
            matchVehicleList();
        }

        private void PrevvehicClicked(object sender, RoutedEventArgs e)
        {
            startIdVehic -= NumberofItemsVehic;
            if (startIdVehic < 0)
                startIdVehic = 0;
            matchVehicleList();
        }

        private void nextvehicClicked(object sender, RoutedEventArgs e)
        {
            startIdVehic += NumberofItemsVehic;
            matchVehicleList();
        }

        private void lastvehicClicked(object sender, RoutedEventArgs e)
        {
            startIdVehic = -NumberofItemsVehic;
            matchVehicleList();
        }

        
        




    }
}
