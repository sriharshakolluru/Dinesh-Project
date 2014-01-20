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
             List<Transaction> trans= CoreOperations.GetAllTransactions(DateTime.Now.AddMonths(-4), DateTime.Now, string.Empty, string.Empty, string.Empty);
             grdTransacData.ItemsSource = trans;
             FillSingleTransacData();
        }

        private void FillSingleTransacData()
        {
            DateTime startDate = (startDatePicker.Value == null) ? default(DateTime) : (DateTime)startDatePicker.Value;
            var trans = CoreOperations.GetAllTransactions(startDate, (DateTime)endDatePicker.Value, txtCustName.Text, txtTech.Text, txtRegID.Text);
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
    }
}
