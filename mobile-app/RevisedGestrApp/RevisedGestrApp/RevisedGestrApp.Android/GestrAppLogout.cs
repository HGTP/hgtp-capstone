/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using Android.App;
using Android.Content;
using Android.Content.PM;

using Okta.Xamarin.Android;

namespace RevisedGestrApp.Droid
{
	[Activity(Label = "GestrApp Logout", NoHistory = true, LaunchMode = LaunchMode.SingleInstance)]
	[
	IntentFilter
	(
		actions: new[] { Intent.ActionView },
		Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
		DataSchemes = new[] { "com.companyname.revisedgestrapp.logout" },
		DataPath = ":/callback"
	)
]
	public class GestrAppLogout : OktaLogoutCallbackInterceptorActivity<MainActivity>
	{

	}
}