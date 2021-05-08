/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

namespace RevisedGestrApp.Models
{
    /// <summary>
    /// Represents the set of information that determines authenticity.
    /// </summary>
    public class AuthInfo
    {
        public string AccessToken { get; set; }
        public string IdToken { get; set; }
        public bool IsAuthorized { get; set; }
        public string RefreshToken { get; set; }
        public string Scope { get; set; }
    }
}
