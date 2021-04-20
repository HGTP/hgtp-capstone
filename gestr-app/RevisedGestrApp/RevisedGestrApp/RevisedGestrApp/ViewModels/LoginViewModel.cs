/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using RevisedGestrApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;
using Xamarin.Essentials;

namespace RevisedGestrApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public string Name { get; private set; }
        public string Preferred { get; private set; }

        public LoginViewModel()
        {
            Title = "Login";
            Name = "";
            Preferred = "";
        }

        public LoginViewModel(string name, string preferred)
        {
            Title = "Login";
            Name = name;
            Preferred = preferred;
            Application.Current.Properties["UserName"] = name;
            Application.Current.Properties["Email"] = preferred;
        }
    }
}
