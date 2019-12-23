using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Model;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Geolocator;
using TravelApp.Logic;

namespace TravelApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TravelPage : ContentPage
    {
        public TravelPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();

            var venues = await VenueLogic.GetVenues(position.Latitude, position.Longitude);
        }

        private void SaveToolbarItem_Clicked(object sender, EventArgs e)
        {
            Post post = new Post()
            {
                Experience = experienceEntry.Text
            };

            using (SQLiteConnection conn = new SQLiteConnection(TravelApp.DatabaseLocation))
            {
                conn.CreateTable<Post>();
                int rows = conn.Insert(post);
                if (rows > 0)
                {
                    DisplayAlert("Success", "Experiance successfully inserted!", "OK");
                }
                else
                {
                    DisplayAlert("Failure", "Experiance failed to be inserted!", "OK");
                }
            }

        }
    }
}