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
using Xamarin.Essentials;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;
using System.Threading.Tasks;
using RevisedGestrApp.Models;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
///Licensed under the MIT license. Read the project readme for details.
/// </summary>
namespace RevisedGestrApp.Droid.Services.GPS
{
    public class GPSManager : IGPSManager
    {
        private const string url = "https://maps.googleapis.com/maps/api/directions/json?origin=";
        private const string url2 = "&destination=";
        private const string url3 = "&mode=";
        private const string url4 = "&key=<INSERT_GOOGLE_API_KEY>";
        private CancellationTokenSource cts;

        /// <summary>
        /// Gets the Next direction in the list and returns it by gettin the settings stored
        /// in the phone and getting the first direction
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetNextDirectionAsync()
        {

            //get GPSInfo saved to phone
            GPSHandler handler = new GPSHandler();
            GPSSetting settings = await handler.GetSetting();

            if(settings == null)
            {
                return "please add a destination and choose a transportation mode on the GPS settings page.";
            }
            //use GPSInfo to get directions
            string getDirection = await GetFirstDirection(settings);

            //return what the app needs to say
            return getDirection;

        }

        /// <summary>
        /// Reads the next direction in the list
        /// </summary>
        public async void ReadNextDirectionAsync()
        {
            string direction = await GetNextDirectionAsync();
            var readAloud = new TextToAudio("Next direction is " + direction);
        }

        /// <summary>
        /// Gets the first text direction using the current GPS Location and 
        /// settings passed in
        /// </summary>
        /// <param name="settings">the settings for where you want to go</param>
        /// <returns>the first direction in the list</returns>
        private async Task<string> GetFirstDirection(GPSSetting settings)
        {
            try
            {
                //make destination string with '+' for spaces
                string dest = settings.Destination;
                dest = dest.Replace(" ", "+");
                Rootobject directions = await GetDirectionsAsync(dest, settings.Mode);
                

                Route firstRoute = directions.routes[0];

                //this is where the steps can be found
                Leg getLeg = firstRoute.legs[0];

                //get first step
                Step firstStep = getLeg.steps[0];

                //get next direction string
                string nextDirection = firstStep.html_instructions;

                //reference: https://stackoverflow.com/questions/787932/using-c-sharp-regular-expressions-to-remove-html-tags
                //From user: Daniel Brückner and edited by verdesmarald
                //This handles most cases for HTML tags but not all as described in the link
                String result = Regex.Replace(nextDirection, @"<[^>]*>", String.Empty);
                
                //add spaces to fix the no spaces between sentences cause by previous regex
                result = Regex.Replace(result, @"Continue", " Continue");
                result = Regex.Replace(result, @"Destination", " Destination");
                result = Regex.Replace(result, @"Toll", " Toll");
                result = Regex.Replace(result, @"Pass", " Pass");
                result = Regex.Replace(result, @"Entering", " Entering");
                result = Regex.Replace(result, @"Restricted", " Restricted");
                //replace edge cases
                result = Regex.Replace(result, @"&nbsq;", " ");

                //assumption is that this text is being read out loud
                //replace things that the text to sound reader doesn't understand
                result = result.Replace("N", "North");
                result = result.Replace("E", "East");
                result = result.Replace("S", "South");
                result = result.Replace("W", "West");
                result = result.Replace("/", " ");
         
                return result;
            }
            catch(Exception e)
            {
                return "Could not get direction. Please check internet connection and try again";
            }
            
        }

       /// <summary>
       /// Gets a Rootobject which represents the main object that
       /// a request to the Google directions API returns
       /// </summary>
       /// <param name="currentLocation"></param>
       /// <param name="dest">the desired destination</param>
       /// <param name="mode">the mode of transportation</param>
       /// <returns></returns>
        public async Task<Rootobject> GetDirectionsAsync(string dest, string mode)
        {
            var client = new System.Net.Http.HttpClient();
            string loc = "";
            //string loc = "https://www.google.com/maps/search/?api=1&query=";
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                    loc += location.Latitude + "," + location.Longitude;
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Unable to get location");
            }

            string finalURL = url + loc + url2 + dest + url3 + mode + url4;
            var response = await client.GetAsync(finalURL);
            string directionsJson = await response.Content.ReadAsStringAsync(); //Getting response  

            Rootobject directions = JsonConvert.DeserializeObject<Rootobject>(directionsJson);
            return directions;
        }


        /// <summary>
        /// Gets the current GPS location
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetGPSLocation()
        {
            //Code from official Xamarin documentation
            //https://docs.microsoft.com/en-us/xamarin/essentials/geolocation?tabs=android
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    return location.Latitude + "," + location.Longitude;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {

                // Handle not supported on device exception
                return "Exception: FeatureNotSupportedException";
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                return "Exception: FeatureNotEnabledException";
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                return "Exception: PermissionException";
            }
            catch (Exception ex)
            {
                // Unable to get location
                return "Exception: Exception";
            }
            return "Unable to get GPS location";
        }

        /// <summary>
        /// builds request for current location
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetGPSCoordinates()
        {
            string loc = "https://www.google.com/maps/search/?api=1&query=";
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                    loc += location.Latitude + "," + location.Longitude;
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Unable to get location");
            }

            return loc;
        }
    }
}