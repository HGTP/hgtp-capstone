/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System;
using System.Collections.Generic;
using System.Text;

namespace RevisedGestrApp.Helpers
{
    public class Constants
    {
        public const string AuthStateKey = "authState";
        public const string AuthServiceDiscoveryKey = "authServiceDiscovery";

        public const string ClientId = "0oaa37zd6FxCsyQnl5d6";
        public const string RedirectUri = "com.okta.dev-76404687:/callback";
        public const string OrgUrl = "https://dev-76404687.okta.com";
        public const string AuthorizationServerId = "default";

        public static readonly string DiscoveryEndpoint =
            $"https://dev-76404687.okta.com/oauth2/default/.well-known/openid-configuration";


        public static readonly string[] Scopes = new string[] {
            "openid", "profile", "email", "offline_access" };

    }
}
