/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using RevisedGestrApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public partial class GPSPage : ContentPage
    {
        //view model for easy access
        private GPSViewModel p;
        bool initializing;

        /// <summary>
        /// Sets up the view model, necessary items,  and buttons for this page
        /// </summary>
        public GPSPage()
        {       
            this.BindingContext = new GPSViewModel();
            p = (GPSViewModel)this.BindingContext;
            InitializeComponent();
            
            if (p.SavedMode.Equals("bicycling"))
            {
                bikeButton.IsChecked = true;
            }else if (p.SavedMode.Equals("driving"))
            {
                carButton.IsChecked = true;
            }else if (p.SavedMode.Equals("transit"))
            {
                transitButton.IsChecked = true;
            }else if (p.SavedMode.Equals("walking"))
            {
                walkButton.IsChecked = true;
            }

            if (ButtonClicked() == null || TextEditor.Text.Equals(""))
            {
                RouteButton.IsEnabled = false;
                SubmitButton.IsEnabled = false;

            }

        }

        /// <summary>
        /// When the submit is changed it updates the buttons for submitting and viewing the route
        /// appropriately and updates the settings to match the new ones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSubmitClicked(object sender, EventArgs e)
        {
            p.UpdateSettingAsync();
            RouteButton.IsEnabled = true;
            SubmitButton.IsEnabled = false;
        }


        /// <summary>
        /// Opens the ViewRoute Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewRouteClickedAsync(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ViewRoutePage());
        }


        /// <summary>
        /// returns the string associated with the buttons for Google Directions API
        /// bicycling,driving,transit,walking, or null if nothing clicked
        /// </summary>
        /// <returns> bicycling,driving,transit,walking, or null if nothing clicked</returns>
        private string ButtonClicked()
        {
            if (bikeButton.IsChecked)
            {
                return "bicycling";
            }
            else if (carButton.IsChecked)
            {
                return "driving";
            }
            else if (transitButton.IsChecked)
            {
                return "transit";
            }else if (walkButton.IsChecked)
            {
                return "walking";
            }
            return null;
        }
        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

            if (p.SavedMode == null)
            {
                SubmitButton.IsEnabled = false;
                RouteButton.IsEnabled = false;
                return;
            }

            if (ButtonClicked() == null ||p.GPSDestination == null || p.GPSDestination.Equals(""))
            {
             
                RouteButton.IsEnabled = false;
            }
            else
            {
                if (!p.SavedMode.Equals(ButtonClicked()))
                {
                    SubmitButton.IsEnabled = true;
                    RouteButton.IsEnabled = false;
                }
                else
                {
                    RouteButton.IsEnabled = true;
                    SubmitButton.IsEnabled = false;
                }
                
            }


        }

        private void TextEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(TextEditor.Text != null && !TextEditor.Text.Equals("") && !TextEditor.Text.Equals(p.SavedDest) && ButtonClicked() != null)
            {
                SubmitButton.IsEnabled = true;
                RouteButton.IsEnabled = false;
                return;
            }

            SubmitButton.IsEnabled = false;
            
        }
    }
}