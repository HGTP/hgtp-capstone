/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RevisedGestrApp.Models;
using RevisedGestrApp.ViewModels;

namespace RevisedGestrApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SOSContactPage : ContentPage
    {
        private EmergencyTextViewModel p;
        public SOSContactPage()
        {
            this.BindingContext = new EmergencyTextViewModel();

            InitializeComponent();
            p = (EmergencyTextViewModel)this.BindingContext;
            RecipientListView.ItemsSource = p.Items;
            TextEditor.Text = p.MessageText;
            GPSCheckBox.IsChecked = p.SendGPSCoords;
        }

        void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null)
                return;

            object currentItem = (object)e.Item;
            Task.Run(() => p.RemoveFromList((Contact)currentItem));

        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            string phoneNumber = await DisplayPromptAsync("Phone Number", "Enter contact's phone number:");

            if (phoneNumber != null && !phoneNumber.Equals(""))
            {
                string name = await DisplayPromptAsync("Contact Name", "Enter contact's name:");

                if (name != null && !name.Equals(""))
                {
                    Contact newContact = new Contact() { Name = name, PhoneNumber = phoneNumber };
                    p.AddToList(newContact);
                }
            }
        }

        private async void OnCkeckboxChanged(object sender, CheckedChangedEventArgs e)
        {
            await Task.Run(() => p.UpdateGPSBool(e.Value));
        }

        private void OnSubmitClicked(object sender, EventArgs e)
        {
            Task.Run(() => p.UpdateContacts());
            Task.Run(() => p.UpdateMessage());
            //TODO change so the page can be edited and users view page after changes
        }

        private void OnRemoveClicked(object sender, EventArgs e)
        {
            var mi = sender as MenuItem;
            Task.Run(() => p.RemoveFromList((Contact)mi.CommandParameter));
        }
    }
}