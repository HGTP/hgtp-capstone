using RevisedGestrApp.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
///Licensed under the MIT license. Read the project readme for details.
/// </summary>
namespace RevisedGestrApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewRoutePage : ContentPage
    {
        //for easy access to the view model
        private ViewRouteViewModel v;

        /// <summary>
        /// Sets up the view model and sets the DirectionListView to the view model's items
        /// </summary>
        public ViewRoutePage()
        {
           
            this.BindingContext = new ViewRouteViewModel();
            InitializeComponent();
     
            v = (ViewRouteViewModel)this.BindingContext;
            DirectionsListView.ItemsSource = v.Items;
           
        }
    }
}
