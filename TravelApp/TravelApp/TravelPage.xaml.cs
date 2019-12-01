using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Model;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TravelPage : ContentPage
    {
        public TravelPage()
        {
            InitializeComponent();
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Post post = new Post()
            {
                Experience = experienceEntry.Text
            };

            SQLiteConnection conn = new SQLiteConnection(TravelApp.DatabaseLocation);
            conn.CreateTable<Post>();
            int rows=conn.Insert(post);
            conn.Close();

            if (rows > 0)
            {
               DisplayAlert("Success", "Experiance successfully inserted!","OK");
            }
            else
            {
                DisplayAlert("Failure", "Experiance failed to be inserted!","OK");
            }    

        }
    }
}