using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using RevisedGestrApp.Models;
using System.Threading.Tasks;
using RevisedGestrApp.Services;
using System.Text.RegularExpressions;
/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
///Licensed under the MIT license. Read the project readme for details.
/// </summary>
namespace RevisedGestrApp.ViewModels
{
    public class ViewRouteViewModel
    {
        public ObservableCollection<Step> Items { get; set; }
        private GPSSetting setting;
      

        public ViewRouteViewModel()
        { 
            Task.Run(InitSettings).Wait();
        }


        /// <summary>
        /// Asynchronously retrieves settings from persistent storage and updates 
        /// the view. 
        /// </summary>
        private void InitSettings()
        {

            Items = new ObservableCollection<Step>();
            MessagingCenter.Subscribe<string>("", "Unsuccessful", message =>
            {
                Step error = new Step();
                error.html_instructions = "Could not find directions. Please check address and try again.";
                Items.Add(error);
            });
            MessagingCenter.Subscribe<string, Rootobject>("", "GetRoute", (sender, root) =>
            {
                Leg leg = root.routes[0].legs[0];
                for (int i = 0; i < leg.steps.Length; i++)
                {
                    Step current = leg.steps[i];
                    String currentDirection = current.html_instructions;

                    //reference: https://stackoverflow.com/questions/787932/using-c-sharp-regular-expressions-to-remove-html-tags
                    //From user: Daniel Brückner and edited by verdesmarald
                    //This handles most cases for HTML tags but not all as described in the link
                    String result = Regex.Replace(currentDirection, @"<[^>]*>", String.Empty);
                    result = Regex.Replace(result, @"Continue", " Continue");
                    result = Regex.Replace(result, @"Destination", " Destination");
                    result = Regex.Replace(result, @"Toll", " Toll");
                    result = Regex.Replace(result, @"Pass", " Pass");
                    result = Regex.Replace(result, @"Entering", " Entering");
                    result = Regex.Replace(result, @"Restricted", " *Restricted");


                    //replace edge cases
                    result = Regex.Replace(result, @"&nbsp;", " ");

                    current.html_instructions = result;

                    //Google maps likes to repeat directions sometimes to schedule prompts
                    //this is unsuitable for the directions list
                    if (i != 0)
                    {
                        Step last = leg.steps[i - 1];
                        //skip this direction
                        string lastDir = last.html_instructions;
                        if (lastDir.Equals(result))
                        {
                            continue;
                        }
                    }
                    Items.Add(current);
                    
                }
            });
           
            MessagingCenter.Send<string>("Send", "GPSRoute");

        }
    }
}
