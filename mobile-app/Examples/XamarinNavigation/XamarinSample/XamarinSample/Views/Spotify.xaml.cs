using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinSample
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Spotify : ContentPage
    {
        public Spotify()
        {
            InitializeComponent();
        }

        async void onConnectButtonClicked(object sender, EventArgs args)
        {
            Console.WriteLine("Hi");
        }

        async void onBackButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopAsync(true);
        }
    }
}