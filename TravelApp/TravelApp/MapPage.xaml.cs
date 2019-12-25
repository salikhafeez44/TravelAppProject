using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using TravelApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        private bool hasLocationPermission = false;
        public MapPage()
        {
            InitializeComponent();
            GetPermissions();
        }

        private async void GetPermissions()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.LocationWhenInUse))
                    {
                        await DisplayAlert("Need your location", "We need to access your location", "OK");
                    }
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.LocationWhenInUse);
                    if (results.ContainsKey(Permission.LocationWhenInUse))
                    {
                        status = results[Permission.LocationWhenInUse];
                    }
                    if (status == PermissionStatus.Granted)
                    {
                        hasLocationPermission = true;
                        locationsMap.IsShowingUser = true;
                        GetLocation();
                    }
                }
                else if (status == PermissionStatus.Granted)
                {
                    hasLocationPermission = true;
                    locationsMap.IsShowingUser = true;
                    GetLocation();
                }
            }

            catch (Exception ex)
            {
               await  DisplayAlert("Error", ex.Message, "OK");
            }
            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (hasLocationPermission)
            {
                var locator = CrossGeolocator.Current;
                locator.PositionChanged += Locator_PositionChanged;
                await locator.StartListeningAsync(TimeSpan.Zero, 100);

                GetLocation();

                using (SQLiteConnection conn = new SQLiteConnection(TravelApp.DatabaseLocation))
                {
                    conn.CreateTable<Post>();
                    var posts = conn.Table<Post>().ToList();

                    DisplayInMap(posts);
                }
            }
        }

        private void DisplayInMap(List<Post> posts)
        {
            foreach (var post in posts)
            {
                try
                {
                    var positions = new Xamarin.Forms.Maps.Position(post.Latitude, post.Longitude);
                    var pin = new Xamarin.Forms.Maps.Pin()
                    {
                        Type = Xamarin.Forms.Maps.PinType.SavedPin,
                        Position = positions,
                        Label = post.VenueName,
                        Address = post.Address

                    };

                    locationsMap.Pins.Add(pin);
                }
                catch (NullReferenceException nre) { }
                catch (Exception ex) { }
            }
            
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CrossGeolocator.Current.StopListeningAsync();
            CrossGeolocator.Current.PositionChanged -= Locator_PositionChanged;
        }

        private void Locator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            MoveMap(e.Position);
        }

        private async void GetLocation()
        {
            if (hasLocationPermission == true)
            {
                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync();
                MoveMap(position);
            }
            
        }

        private void MoveMap(Position position)
        {
            var center = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
            var span = new Xamarin.Forms.Maps.MapSpan(center, 1, 1);
            locationsMap.MoveToRegion(span);
        }
    }
}