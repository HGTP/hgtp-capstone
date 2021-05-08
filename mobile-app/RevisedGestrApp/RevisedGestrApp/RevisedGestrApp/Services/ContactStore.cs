/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using RevisedGestrApp.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RevisedGestrApp.Services
{
    /// <summary>
    /// This represents a general contact person, not an emergency contact.
    /// </summary>
    public class ContactStore : IListableDataStore<Contact>
    {
        /// <summary>
        /// Adds a single contact.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddAsync(Contact data)
        {
            var contacts = await ListAsync();
            foreach (var contact in contacts)
            {
                if (contact.Name.Equals(data.Name))
                {
                    // This contact has already been added, so we're done.
                    return;
                }
            }
            // At this point we know the contact isn't already there, so we're free to add it.
            contacts.Add(data);
            string contactsJson = JsonConvert.SerializeObject(contacts);
            Application.Current.Properties["ContactList"] = contactsJson;
            await Application.Current.SavePropertiesAsync();
        }

        /// <summary>
        /// Replaces the current contacts with the provided contacts.
        /// 
        /// If data is null, this will clear the list of contacts.
        /// 
        /// Use with caution!
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task ReplaceAsync(ObservableCollection<Contact> data)
        {
            if (data == null)
            {
                Clear();
            }
            else
            {
                string contacts = JsonConvert.SerializeObject(data);
                Application.Current.Properties["ContactList"] = contacts;
                await Application.Current.SavePropertiesAsync();
            }
        }

        /// <summary>
        /// Deletes the contact whose name matches the provided id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string id)
        {
            var contacts = await ListAsync();
            Contact contactToRemove = null;
            foreach (var contact in contacts)
            {
                if (contact.Name.Equals(id))
                {
                    contactToRemove = contact;
                    break;
                }
            }
            if (contactToRemove != null)
            {
                contacts.Remove(contactToRemove);
            }
            await ReplaceAsync(contacts);
        }

        /// <summary>
        /// Deletes all stored contacts.
        /// </summary>
        /// <returns></returns>
        public Task Clear()
        {
            return Task.Run(() =>
            {
                Application.Current.Properties["ContactList"] = null;
            });
        }

        /// <summary>
        /// Returns the contact whose name matches the provided string.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Contact> GetAsync(string id)
        {
            return Task.Run(() =>
            {
                var data = (string)Application.Current.Properties["ContactList"];
                ObservableCollection<Contact> contacts = JsonConvert.DeserializeObject<ObservableCollection<Contact>>(data);
                foreach (var contact in contacts)
                {
                    if (contact.Name.Equals(id))
                    {
                        return contact;
                    }
                }
                return null;
            });
        }

        /// <summary>
        /// Returns all the contacts.
        /// </summary>
        /// <returns></returns>
        public Task<ObservableCollection<Contact>> ListAsync()
        {
            return Task.Run(() =>
            {
                var data = (string)Application.Current.Properties["ContactList"];
                ObservableCollection<Contact> contacts = JsonConvert.DeserializeObject<ObservableCollection<Contact>>(data);
                return contacts;
            });
        }

        /// <summary>
        /// Updates a specific contact with the given data.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Contact data)
        {
            var contacts = await ListAsync();
            foreach (var contact in contacts)
            {
                if (contact.Name.Equals(data.Name))
                {
                    contact.PhoneNumber = data.PhoneNumber;
                }
            }
            string contactsJson = JsonConvert.SerializeObject(contacts);
            Application.Current.Properties["ContactList"] = contactsJson;
            await Application.Current.SavePropertiesAsync();
        }
    }
}
