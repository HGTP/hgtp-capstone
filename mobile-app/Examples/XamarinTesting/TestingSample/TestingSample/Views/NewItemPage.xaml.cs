using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TestingSample.Models;
using TestingSample.ViewModels;

namespace TestingSample.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
			// This is where the view model is bound to this view.
            BindingContext = new NewItemViewModel();
        }
    }
}