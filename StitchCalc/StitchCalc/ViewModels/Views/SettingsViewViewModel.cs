using Acr.DeviceInfo;
using ReactiveUI;
using StitchCalc.Services.NavigationService;
using StitchCalc.Services.SettingsServices;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels.Views
{
	public class SettingsViewViewModel : ViewModelBase, INavigable
	{
		string defaultHourlyCharge;
		string appVersion;

		public SettingsViewViewModel()
		{
			defaultHourlyCharge = SettingsService.Current.DefaultHourlyCharge.ToString();
			appVersion = DeviceInfo.App.ShortVersion;

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

		public string AppVersion => appVersion;
		public string DefaultHourlyCharge
		{
			get { return defaultHourlyCharge; }
			set { this.RaiseAndSetIfChanged(ref defaultHourlyCharge, value); }
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
