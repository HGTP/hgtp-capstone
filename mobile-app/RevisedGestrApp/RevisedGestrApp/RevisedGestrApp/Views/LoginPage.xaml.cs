/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using RevisedGestrApp.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RevisedGestrApp.Services;
using System.IdentityModel.Tokens.Jwt;
using Xamarin.Essentials;
using Okta.Xamarin;
using RevisedGestrApp.Models;

namespace RevisedGestrApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            this.BindingContext = new LoginViewModel();
            InitializeComponent();

            Application.Current.Properties.TryGetValue("UserName", out object name);
            if (name is string)
            {
                WelcomeLabel.Text = "Welcome, " + name.ToString();
                WelcomeLabel.HorizontalOptions = LayoutOptions.Center;
                WelcomeLabel.IsEnabled = true;
            }
        }
        
        /// <summary>
        /// Opens the FAQ page of the website.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OpenFAQClicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.hgtp-capstone.com/faq");
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await OktaContext.Current.SignOutAsync();
        }
    }
}