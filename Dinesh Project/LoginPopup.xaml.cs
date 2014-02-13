﻿using System;
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
using DataOperations;

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
                if (isAuthPassed)
                    this.Close();
                else
                {
                    txtLoginId.Clear();
                    txtPassword.Clear();
                    MessageBox.Show("Authentication Failed","Fail",MessageBoxButton.OK,MessageBoxImage.Error,MessageBoxResult.OK,MessageBoxOptions.DefaultDesktopOnly);
                }
                
            }
        }
        private void isAuthenticationPassed()
        {
            string userName = txtLoginId.Text;
            string password=txtPassword.Password;
            using (CoreDbEntities db = new CoreDbEntities())
            {
                var userDet= db.PasswordDetails.Where(u=>u.LoginID.Equals(userName));
                if(userDet.Any())
                {
                    string strdHs = userDet.First().Password;
                    isAuthPassed= Utility.ValidateMD5HashData(password, strdHs);
                }
            }

        }

        private void BeforeClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!isAuthPassed)
                Environment.Exit(0);

        }
    }
}
