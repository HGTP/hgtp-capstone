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

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Okta.Xamarin;
using Okta.Xamarin.Android;

namespace RevisedGestrApp.Droid
{
	[Activity(Label = "GestrApp Login", NoHistory = true, LaunchMode = LaunchMode.SingleInstance)]
	[
	IntentFilter
	(
		actions: new[] { Intent.ActionView },
		Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
		DataSchemes = new[] { "com.companyname.revisedgestrapp.login" },
		DataPath = ":/callback"
	)
]
	public class GestrAppLogin : OktaLoginCallbackInterceptorActivity<MainActivity>
	{

	}
}
