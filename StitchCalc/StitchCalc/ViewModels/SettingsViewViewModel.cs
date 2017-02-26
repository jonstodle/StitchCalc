﻿using Acr.DeviceInfo;
using ReactiveUI;
using StitchCalc.Services.SettingsServices;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels
{
	public class SettingsViewViewModel : ViewModelBase
	{
		public SettingsViewViewModel()
		{
			_defaultHourlyCharge = SettingsService.Current.DefaultHourlyCharge.ToString();
			_appVersion = DeviceInfo.App.ShortVersion;

			this
				.WhenAnyValue(x => x.DefaultHourlyCharge)
				.Throttle(TimeSpan.FromMilliseconds(500))
				.Where(x => x.IsValidDouble())
				.Select(x => double.Parse(x))
				.Where(x => x >= 0)
				.Subscribe(x => SettingsService.Current.DefaultHourlyCharge = x);
			SettingsService.Current.Changed
				.Where(x => x.Key == nameof(SettingsService.Current.DefaultHourlyCharge))
				.Select(x => x.Value)
				.Cast<double>()
				.Subscribe(x => DefaultHourlyCharge = x.ToString());
		}

		public string AppVersion => _appVersion;
		public string DefaultHourlyCharge
		{
			get { return _defaultHourlyCharge; }
			set { this.RaiseAndSetIfChanged(ref _defaultHourlyCharge, value); }
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;



        private string _defaultHourlyCharge;
        private string _appVersion;
    }
}
