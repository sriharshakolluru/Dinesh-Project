using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Configuration;
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
        Transaction currentTransaction;
        int startItem=0, maxTransactionsPerGrid;
        List<Transaction> currentMappedList;

        public Transactions()
        {
            InitializeComponent();
            maxTransactionsPerGrid = int.TryParse(ConfigurationManager.AppSettings["MaxRowsPerGrid"], out maxTransactionsPerGrid) ? maxTransactionsPerGrid : 5;
            
            txtOperations.ItemsSource = CoreOperations.GetAllOPerations();
            txtOperations.ValueMemberPath = "Name";
            txtTech.ItemsSource = CoreOperations.GetAllTechnicians();
            txtTech.ValueMemberPath = "Name";
            txtRegID.ItemsSource = CoreOperations.GetAllVehicles();
            txtRegID.ValueMemberPath = "RegistrationNumber";
            BindTransactionToData();
        }

        private void  BindTransactionToData()
        {
             List<Transaction> trans= CoreOperations.GetAllTransactions(default(DateTime), DateTime.MaxValue, string.Empty, string.Empty, string.Empty,string.Empty,string.Empty);
             currentMappedList = trans;
             matchTransSearchRequest();
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
            try
            {
                if (string.IsNullOrEmpty(txtServiceID.Text))
                {
                    //Step 1. Check if vehicleExists
                    #region Getting Vehicle
                    if (!string.IsNullOrEmpty(txtRegID.Text))
                    {
                        vehicID = CoreOperations.doesVehicleExist(txtRegID.Text);
                        if (vehicID <= -1)
                        {
                            #region getOwnerID

                            //Step 2. If owner does not exists, addnew owner else get ID and create a new vehicle Data
                            if (!string.IsNullOrEmpty(txtCustName.Text))
                            {
                                ownerID = CoreOperations.doesOwnerExists(txtCustName.Text);

                                if (ownerID <= -1)//create one
                                {
                                    ownerID = CoreOperations.AddANewCustomer(txtCustName.Text, Utility.CreateRandomID(txtCustName.Text)
                                        , txtCustPhone.Text, string.Empty);

                                    if (ownerID <= -1)
                                    {
                                        MessageBox.Show("Error Occurred... Could not create the customer account", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                        return;
                                    }
                                }

                                Customer owner = CoreOperations.GetAllOwnersByName(txtCustName.Text, txtCustPhone.Text).First();
                                ownerID = (int)owner.CustomerID;
                            }
                            else
                            {
                                //when owners data is not available
                                MessageBox.Show("Enter Customer Name");
                                Utility.WriteLogError("Exception Occurred. Owner's Data not specified in UI");
                                return;
                            }
                            #endregion

                            bool vehicleAdded = CoreOperations.AddANewVehicle(txtRegID.Text, string.Empty, (int)ownerID);
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
                            bool operationAdded = CoreOperations.AddANewOperation(txtOperations.Text, string.Empty);
                            if (operationAdded)
                                oprationID = CoreOperations.doesOperationExists(txtOperations.Text);
                        }
                        if (oprationID <= -1)
                        {
                            Utility.WriteLogError("Error: Could not add operation");
                            return;
                        }
                    }
                    #endregion

                    // Step 4. Now create a transaction.
                    DateTime startTime = (startDatePicker.Value.HasValue) ? startDatePicker.Value.Value : default(DateTime);
                    CoreOperations.StartANewTransactionWithExistingVehicle(oprationID, startTime, string.Empty, vehicID, txtTech.Text, string.Empty
                        , txtPaymentDetails.Text, double.Parse(txtPayment.Text), string.Empty);
                    MessageBox.Show("Transaction is Added Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else
                {
                    #region Edit Transaction
                    
                    string serviceID = txtServiceID.Text;
                    List<Transaction> currenttrans = CoreOperations.GetAllTransactions(default(DateTime), default(DateTime), string.Empty, string.Empty, string.Empty, serviceID, string.Empty);
                    Technician tech; Customer cust;
                    

                    #region Check Technician
                    if (CoreOperations.doesTechnicianExists(txtTech.Text)<0)
                    {
                        MessageBoxResult result=MessageBox.Show(string.Format("The technician {0} does not exist in records. Create a new one?",txtTech.Text)
                                                                                                , "Respond", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result.Equals(MessageBoxResult.Yes))
                        {
                            CoreOperations.AddANewTechnician(txtTech.Text, Utility.CreateRandomID(txtTech.Text));
                            Utility.WriteLog("New Customer Added Before Adding it to Transaction");
                        }
                        else
                        {
                            MessageBox.Show("Add Technician Manually or retry with existing technician. Terminating the transaction"
                                                                                        ,"Terminating",MessageBoxButton.OK,MessageBoxImage.Stop);
                            return;
                        }
                    }
                    tech = CoreOperations.GetAllTechnicians(txtTech.Text, string.Empty).First();
                    #endregion

                    
                    #region check customer
                    if (CoreOperations.doesOwnerExists(txtCustName.Text) < 0)
                    {
                        MessageBoxResult result = MessageBox.Show(string.Format("The Customer {0} does not exist in records. Create a new one?", txtCustName.Text)
                                                                                                , "Respond", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result.Equals(MessageBoxResult.Yes))
                        {
                            CoreOperations.AddANewCustomer(txtTech.Text, txtRegID.Text, txtCustPhone.Text, string.Empty);
                            Utility.WriteLog("New Customer Added Before Adding it to Transaction");
                        }
                        else
                        {
                            MessageBox.Show("Add A Customer manually from report pages. Transaction is Terminated");
                            return;
                        }
                    }
                    else
                    {
                        cust =CoreOperations.GetAllOwnersByName(txtCustName.Text, string.Empty).First();
                        if (!cust.Phone.Equals(txtCustPhone.Text))
                        {
                            bool editSuccess=CoreOperations.EditACustomer((int)cust.CustomerID, cust.RegistrationID, txtCustName.Text, txtCustPhone.Text, cust.Address);
                            if (editSuccess)
                                Utility.WriteLog("Customer edited as new phone number entered in the phone");
                            else
                                Utility.WriteLog("Customer Edit is failed. Could not edit the customer");
                        }
                        
                    }
                    cust = CoreOperations.GetAllOwnersByName(txtCustName.Text, string.Empty).First();

                    #endregion


                    #region check Operation
                    if ((CoreOperations.doesOperationExists(txtOperations.Text) <= -1))
                        if (CoreOperations.AddANewOperation(txtOperations.Text, string.Empty))
                        {
                            Utility.WriteLog("Added a new Operation: " + txtOperations.Text);
                        }
                        else
                        {
                            Utility.WriteLog("Could not add new Operation: " + txtOperations.Text);
                            MessageBox.Show("Adding New Opeartion failed. Continuing with no Operation Data. Contact Admin Immediately");
                        }

                    oprationID = CoreOperations.doesOperationExists(txtOperations.Text);
                    //***Not yet Implemented
                    #endregion
                    if (currenttrans != null && currenttrans.Count > 0 && currentTransaction != null)
                    {
                        DateTime endTime = (endDatePicker.Value.HasValue) ? endDatePicker.Value.Value : default(DateTime);
                        bool isSuccessful = CoreOperations.EditATransaction(serviceID, txtRegID.Text,(int) cust.CustomerID, tech.Id, txtPaymentDetails.Text, double.Parse(txtPayment.Text), startDatePicker.Value.Value, endTime,oprationID);
                        if (isSuccessful)
                        {
                            MessageBox.Show(string.Format("Transaction with service ID {0}is Edited Successfully", serviceID)
                                                        , "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show(string.Format("Any Transaction with Service ID Cannot be found.",serviceID));
                    }
                    #endregion
                }
                BindTransactionToData();
            }
            catch (Exception ex)
            {                
                Utility.WriteLogError("Exception Occurred in edit/add a transaction " + ex.ToString());
                MessageBox.Show(string.Format("Transaction Edit Failed. Contact Administrator"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);   
            }
        }


        /// <summary>
        /// Tested
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearTransacData(object sender, RoutedEventArgs e)
        {
            startItem = 0;
            txtCustName.Text = string.Empty;
            txtOperations.Text = string.Empty;
            txtTech.Text = string.Empty;
            startDatePicker.Value = null;
            endDatePicker.Value = null;
            txtPayment.Text = string.Empty;
            txtPaymentDetails.Text = string.Empty;
            txtRegID.Text = string.Empty;
            txtCustPhone.Text = string.Empty;
            txtServiceID.Text = string.Empty;
            matchTransSearchRequest();
        }

        private void deleteTransaction(object sender, RoutedEventArgs e)
        {

        }

        private void searchTransactions(object sender, RoutedEventArgs e)
        {
            startItem = 0;
            string registrationID = txtRegID.Text;
            string ownerName = txtCustName.Text;
            DateTime startDate = (startDatePicker.Value.HasValue &&(DateTime.Now-startDatePicker.Value.Value).TotalMinutes>5) ? startDatePicker.Value.Value : default(DateTime);
            DateTime endDate = (endDatePicker.Value.HasValue && ((DateTime.Now-endDatePicker.Value.Value).TotalMinutes>5)) ? endDatePicker.Value.Value : default(DateTime);
            string technicianName = txtTech.Text;
            List<Transaction> matchdList = CoreOperations.GetAllTransactions(startDate, endDate, ownerName, technicianName, registrationID, string.Empty, txtCustPhone.Text);
            currentMappedList = matchdList;
            matchTransSearchRequest();
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
                    currentTransaction = selectedTransac;
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
                    txtServiceID.Text = selectedTransac.ServiceId;
                }

            }
        }

        private void FirstTransCustCliked(object sender, RoutedEventArgs e)
        {
            startItem = 0;
            matchTransSearchRequest();
        }

        private void matchTransSearchRequest()
        {
            var finalLIst = currentMappedList.Skip(startItem).Take(maxTransactionsPerGrid);
            grdTransacData.ItemsSource = finalLIst;
        }

        private void PrevTransClicked(object sender, RoutedEventArgs e)
        {
            startItem -= maxTransactionsPerGrid;
            if (startItem < 0)
                startItem = 0;

            matchTransSearchRequest();
        }

        private void nextTransClicked(object sender, RoutedEventArgs e)
        {
            int tempStart = startItem;
            startItem += maxTransactionsPerGrid;
            if (startItem > currentMappedList.Count)
                startItem = tempStart;
            if (startItem < 0)
                startItem = 0;

            matchTransSearchRequest();
        }

        private void lastTransClicked(object sender, RoutedEventArgs e)
        {
            startItem = currentMappedList.Count - maxTransactionsPerGrid;
            if (startItem < 0)
                startItem = 0;
            matchTransSearchRequest();

        }

        private void TxtCustomerFocusLost(object sender, RoutedEventArgs e)
        {
            var matchedCustoemrs = CoreOperations.GetAllOwnersByName(txtCustName.Text, string.Empty);
            if(matchedCustoemrs.Any())
                txtCustPhone.Text= matchedCustoemrs.First().Phone;
        }
        

    }
}
