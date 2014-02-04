using System;
using System.Collections.Generic;
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
using System.Data;
using DataOperations;

namespace Dinesh_Project
{
    /// <summary>
    /// Interaction logic for Transactions.xaml
    /// </summary>
    public partial class Transactions : Page
    {
        public Transactions()
        {
            InitializeComponent();
            BindTransactionToData();
            txtOperations.ItemsSource = CoreOperations.GetAllOPerations();
            txtOperations.ValueMemberPath = "Name";
            txtTech.ItemsSource = CoreOperations.GetAllTechnicians();
            txtTech.ValueMemberPath = "Name";


        }

        private void  BindTransactionToData()
        {
             List<Transaction> trans= CoreOperations.GetAllTransactions(DateTime.Now.AddMonths(-4), DateTime.Now, string.Empty, string.Empty, string.Empty,string.Empty,string.Empty);
             grdTransacData.ItemsSource = trans;
            // FillSingleTransacData();
        }

        private void FillSingleTransacData()
        {
            DateTime startDate = (startDatePicker.Value == null) ? default(DateTime) : (DateTime)startDatePicker.Value;
            var trans = CoreOperations.GetAllTransactions(startDate, (DateTime)endDatePicker.Value, txtCustName.Text, txtTech.Text, txtRegID.Text,string.Empty,string.Empty);
            if (trans.Count > 0)
            {
                var transaction = trans.First();
                txtCustName.Text = transaction.Vehicle.Customer.Name;
                txtOperations.Text = transaction.Operation.Name;
                txtTech.Text = transaction.Technician.Name;
                startDatePicker.Value = transaction.StartDate;
                endDatePicker.Value = transaction.EndDate;
                txtPayment.Text = transaction.PaymentAmount.ToString();
                txtPaymentDetails.Text = transaction.PaymentStatus;
                txtRegID.Text = transaction.Vehicle.RegistrationNumber;
                txtCustPhone.Text = transaction.Vehicle.Customer.Phone;
            }
            else
            {
                MessageBox.Show("NO transaction Matched the search criteria");
            }
        }

        private void saveNewTransaction(object sender, RoutedEventArgs e)
        {

            int ownerID = -1, oprationID = -1, vehicID=-1;
            //Step 1. Check if vehicleExists
            #region Getting Vehicle
            if (!string.IsNullOrEmpty(txtRegID.Text))
            {
                vehicID = CoreOperations.doesVehicleExist(txtRegID.Text);
                if(vehicID<=-1)
                {
                    #region getOwnerID
                    
                    //Step 2. If owner does not exists, addnew owner else get ID and create a new vehicle Data
                    if (!string.IsNullOrEmpty(txtCustName.Text))
                    {
                        ownerID=CoreOperations.doesOwnerExists(txtCustName.Text);
                        
                        if(ownerID <= -1)//create one
                        {
                            ownerID= CoreOperations.AddANewCustomer(txtCustName.Text, Utility.CreateRandomID(txtCustName.Text)
                                , txtCustPhone.Text, string.Empty);
                            
                            if(ownerID <= -1)
                            {                                
                                MessageBox.Show("Error Occurred... Could not create the customer account", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;   
                            }
                        }
                        
                        Customer owner= CoreOperations.GetAllOwnersByName(txtCustName.Text,txtCustPhone.Text).First();
                        ownerID =(int) owner.CustomerID;
                    }
                    else
                    {
                        //when owners data is not available
                        MessageBox.Show("Enter Customer Name");
                        Utility.WriteLogError("Exception Occurred. Owner's Data not specified in UI");
                        return;
                    }
                    #endregion

                    bool vehicleAdded= CoreOperations.AddANewVehicle(txtRegID.Text, string.Empty,(int) ownerID);
                    if (!vehicleAdded)
                    {
                        MessageBox.Show("Error Occurred... Could not create the Vehicle Data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                vehicID = CoreOperations.doesVehicleExist(txtRegID.Text);

            }
            #endregion

            //Step 3. Check if the operation Exists
            #region GetOperation 
            
            if (string.IsNullOrEmpty(txtOperations.Text))
                MessageBox.Show("Enter The Operation Name", "Missing Value", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                oprationID = CoreOperations.doesOperationExists(txtOperations.Text);
                if (oprationID <= -1)
                {
                    bool operationAdded=CoreOperations.AddANewOperation(txtOperations.Text, string.Empty);
                    if (operationAdded)
                        oprationID= CoreOperations.doesOperationExists(txtOperations.Text);
                }
                if (oprationID <= -1)
                {
                    Utility.WriteLogError("Error: Could not add operation");
                    return;
                }
            }
            #endregion

            // Step 4. Now create a transaction.
            DateTime startTime=(startDatePicker.Value.HasValue)?startDatePicker.Value.Value:default(DateTime);
            CoreOperations.StartANewTransactionWithExistingVehicle(oprationID, startTime, string.Empty,vehicID, txtTech.Text, string.Empty
                , txtPaymentDetails.Text, double.Parse(txtPayment.Text), string.Empty);

            

        }


        /// <summary>
        /// Tested
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearTransacData(object sender, RoutedEventArgs e)
        {
            txtCustName.Text = string.Empty;
            txtOperations.Text = string.Empty;
            txtTech.Text = string.Empty;
            startDatePicker.Value = null;
            endDatePicker.Value = null;
            txtPayment.Text = string.Empty;
            txtPaymentDetails.Text = string.Empty;
            txtRegID.Text = string.Empty;
            txtCustPhone.Text = string.Empty;

        }

        private void deleteTransaction(object sender, RoutedEventArgs e)
        {

        }

        private void searchTransactions(object sender, RoutedEventArgs e)
        {
            string registrationID = txtRegID.Text;
            string ownerName = txtCustName.Text;
            DateTime startDate = (startDatePicker.Value.HasValue) ? startDatePicker.Value.Value : default(DateTime);
            DateTime endDate = (endDatePicker.Value.HasValue) ? endDatePicker.Value.Value : default(DateTime);
            string technicianName = txtTech.Text;
            List<Transaction> matchdList= CoreOperations.GetAllTransactions(startDate,endDate,ownerName,technicianName,registrationID,string.Empty,txtCustPhone.Text);
            grdTransacData.ItemsSource = matchdList;
                
        }

        /// <summary>
        /// Tested..
        /// When transaction is selected, it will get the transaction details into the textboxes and datepickers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectTransaction(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;
                if (grid != null && grdTransacData.SelectedItems != null && grdTransacData.SelectedItems.Count == 1)
                {
                    Transaction selectedTransac = (Transaction) grdTransacData.SelectedItems[0];
                    //string serviceID = row["ServiceID"].ToString();
                    //Transaction selectedTransac= CoreOperations.GetAllTransactions(default(DateTime), default(DateTime), string.Empty, string.Empty, string.Empty, serviceID).First();
                    txtCustName.Text = selectedTransac.Vehicle.Customer.Name;
                    txtOperations.Text = selectedTransac.Operation.Name;
                    txtTech.Text = selectedTransac.Technician.Name;
                    startDatePicker.Value = selectedTransac.StartDate;
                    endDatePicker.Value = selectedTransac.EndDate;
                    txtPayment.Text = selectedTransac.PaymentAmount.Value.ToString();
                    txtPaymentDetails.Text = selectedTransac.PaymentStatus;
                    txtRegID.Text = selectedTransac.Vehicle.RegistrationNumber;
                    txtCustPhone.Text = selectedTransac.Vehicle.Customer.Phone;
                }

            }
        }
        

    }
}
