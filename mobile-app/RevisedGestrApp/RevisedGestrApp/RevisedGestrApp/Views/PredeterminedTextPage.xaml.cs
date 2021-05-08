/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RevisedGestrApp.Models;
using RevisedGestrApp.ViewModels;

namespace RevisedGestrApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PredeterminedTextPage : ContentPage
    {
        private PredeterminedTextViewModel p;
        public PredeterminedTextPage()
        {
            this.BindingContext = new PredeterminedTextViewModel();
            
            InitializeComponent();
            p = (PredeterminedTextViewModel)this.BindingContext;
            RecipientListView.ItemsSource = p.Items;
            TextEditor.Text = p.MessageText;
        }

        async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null)
                return;

            object currentItem = (object)e.Item;
            p.RemoveFromList((Contact)currentItem);

        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            //string action = await DisplayActionSheet("Add Contact", "Cancel", null, "Manually", "From Contacts");

            //if (action.Equals("Manually"))
            //{
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

            //}
            //else
            //{
                //add Contacts code for this
            //}
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            this.p.UpdateContacts();
            this.p.UpdateMessage();
            //TODO change so the page can be edited and users view page after changes
        }

        private async void OnRemoveClicked(object sender, EventArgs e)
        {
            var mi = sender as MenuItem;
            p.RemoveFromList((Contact)mi.CommandParameter);
        }
    }
}