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
            venuListView.ItemsSource = venues;
        }

        private void SaveToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                var selectedVenue = venuListView.SelectedItem as Venue;
                var firstCategory = selectedVenue.categories.FirstOrDefault();
                Post post = new Post()
                {
                    Experience = experienceEntry.Text,
                    VenueName = selectedVenue.name,
                    CategoryId = firstCategory.id,
                    CategoryName = firstCategory.name,
                    Address = selectedVenue.location.address,
                    Distance = selectedVenue.location.distance,
                    Latitude = selectedVenue.location.lat,
                    Longitude = selectedVenue.location.lng,

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
            catch (NullReferenceException nre)
            {

            }
            catch(Exception ex)
            {

            }

        }
    }
}