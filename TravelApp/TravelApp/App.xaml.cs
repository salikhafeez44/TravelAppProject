using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelApp
{
    public partial class TravelApp : Application
    {
        public static string DatabaseLocation = string.Empty;
        public TravelApp()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }
        public TravelApp(string databaseLocation)
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
            DatabaseLocation = databaseLocation;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
