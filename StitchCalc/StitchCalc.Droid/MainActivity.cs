using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;

namespace StitchCalc.Droid
{
	[Activity (Label = "StitchCalc", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			UserDialogs.Init(this);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication (new StitchCalc.App ());
		}
	}
}

