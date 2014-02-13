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
using System.Windows.Shapes;

namespace Dinesh_Project
{
    /// <summary>
    /// Interaction logic for LoginPopup.xaml
    /// </summary>
    public partial class LoginPopup : Window
    {

        bool isAuthPassed = false;
        public LoginPopup()
        {
            InitializeComponent();
        }

        private void loginId_keyDown(object sender, KeyEventArgs e)
        {
            if(e.Key.Equals(Key.Enter))
            {
                txtPassword.Focus();
            }
        }
        
        

        private void key_down(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                isAuthenticationPassed();
                this.Close();
            }
        }
        private void isAuthenticationPassed()
        {
            string userName = txtLoginId.Text;
            string password=txtPassword.Password;
            using (CoreDbEntities db = new CoreDbEntities())
            {
                db.PasswordDetails.Where(u=>u.LoginID.Equals(userName));
            }

            if (userName.Equals(password))
                isAuthPassed = true;
        }

        private void BeforeClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Environment.Exit(0);
        }
    }
}
