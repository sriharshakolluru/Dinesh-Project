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
                
            }
        }
        private bool isAuthenticationPassed()
        {
            string userName = txtLoginId.Text;
            string password=txtPassword.Password;

            return false;
        }
    }
}
