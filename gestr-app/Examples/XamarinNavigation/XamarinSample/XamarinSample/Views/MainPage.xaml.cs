﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void onSpotifyButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new Spotify(), true);
        }
    }
}
