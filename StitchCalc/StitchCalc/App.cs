﻿using System;
using System.IO;
using StitchCalc.Views;

using Xamarin.Forms;
using StitchCalc.Services;

namespace StitchCalc
{
	public class App : Application
	{
		public App ()
		{
			AddGlobalResources();

			// The root page of your application
			MainPage = new NavigationPage(new HomeTabsView());

            NavigationService.Init(MainPage.Navigation);
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}



		void AddGlobalResources()
		{
			Resources = new ResourceDictionary();

			Resources.Add("StandardPadding", new Thickness(
				Device.OnPlatform(10, 10, 0),
				Device.OnPlatform(10, 10, 0)));
		}
	}
}
