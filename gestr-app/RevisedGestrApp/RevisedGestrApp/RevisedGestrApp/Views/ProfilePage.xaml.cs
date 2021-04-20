/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RevisedGestrApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            UserNameLabel.Text = "Username: " + (string)Application.Current.Properties["UserName"];
            EmailLabel.Text = "Email: " + (string)Application.Current.Properties["Email"];
        }

        private async void ChangePasswordClicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://dev-76404687.okta.com/enduser/settings/");
        }
    }
}