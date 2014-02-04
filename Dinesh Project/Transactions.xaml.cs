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

        }

        private void  BindTransactionToData()
        {
             List<Transaction> trans= CoreOperations.GetAllTransactions(DateTime.Now.AddMonths(-4), DateTime.Now, string.Empty, string.Empty, string.Empty,string.Empty);
             grdTransacData.ItemsSource = trans;
            // FillSingleTransacData();
        }

        private void FillSingleTransacData()
        {
            DateTime startDate = (startDatePicker.Value == null) ? default(DateTime) : (DateTime)startDatePicker.Value;
            var trans = CoreOperations.GetAllTransactions(startDate, (DateTime)endDatePicker.Value, txtCustName.Text, txtTech.Text, txtRegID.Text,string.Empty);
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
            }
            else
            {
                MessageBox.Show("NO transaction Matched the search criteria");
            }
        }

        private void saveNewTransaction(object sender, RoutedEventArgs e)
        {
            //Step 1. Check if vehicleExists
            //if(string.IsNullOrEmpty(txtRegID.te)


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
            List<Transaction> matchdList= CoreOperations.GetAllTransactions(startDate,endDate,ownerName,technicianName,registrationID,string.Empty);
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

                }

            }
        }
        

    }
}
