using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TravelApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void LogIn_Clicked(object sender, EventArgs e)
        {
            bool isemailEmpty = string.IsNullOrEmpty(emailEntry.Text);
            bool ispasswordEmpty = string.IsNullOrEmpty(passwordEntry.Text);

            if (isemailEmpty || ispasswordEmpty)
            {
                loginLabel.Text = "Email or Password not entered!";
            }
            else
            {
                loginLabel.Text = "Login Successful!";
                
            }
        }
    }
}