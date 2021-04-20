using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace BTPairing
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnSearchDevicesButtonClicked(object sender, EventArgs e)
        {
            //Request location permission from user if user hasn't allowed yet
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }
    }
}
