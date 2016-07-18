using ReactiveUI;
using StitchCalc.Extras;
using StitchCalc.Services.NavigationService;
using StitchCalc.Services.SettingsServices;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels.Views
{
	public class SettingsViewViewModel : ViewModelBase, INavigable
	{
		string defaultHourlyCharge;

		public SettingsViewViewModel()
		{
			defaultHourlyCharge = SettingsService.Current.DefaultHourlyCharge.ToString();

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
