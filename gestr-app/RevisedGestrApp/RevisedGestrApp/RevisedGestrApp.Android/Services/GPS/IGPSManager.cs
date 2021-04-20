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
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
///Licensed under the MIT license. Read the project readme for details.
/// </summary>
namespace RevisedGestrApp.Droid.Services.GPS
{
    public interface IGPSManager
    {
        Task<string> GetNextDirectionAsync();
        void ReadNextDirectionAsync();
        Task<string> GetGPSCoordinates();
    }
}