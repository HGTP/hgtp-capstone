/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

namespace RevisedGestrApp.Models
{
    /// <summary>
    /// Represents a setting for a gesture and its action.
    /// </summary>
    public class Setting
    {
        public string Gesture { get; set; }
        public string PhoneAction { get; set; }
    }
}
