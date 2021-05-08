using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinSample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Texting : ContentPage
    {
        public Texting()
        {
            InitializeComponent();
        }
        async void onSOSButtonClicked(object sender, EventArgs args)
        {
            Console.WriteLine("Hi");
        }

        async void onBackButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopAsync(true);
        }
    }
}