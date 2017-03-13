using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace StitchCalc.Services
{
	public partial class SettingsService
	{
		public static SettingsService Current { get { return _current != null ? _current : _current = new SettingsService(); } }



		SettingsService()
		{
			settings = CrossSettings.Current;
			changed = new Subject<KeyValuePair<string, object>>();
		}



		public IObservable<KeyValuePair<string, object>> Changed => changed;

		// App settings
		public bool IsFirstRun
		{
			get { return Read(nameof(IsFirstRun), true); }
			set { Write(nameof(IsFirstRun), value); }
		}

		// User Settings
		public double DefaultHourlyCharge
		{
			get { return Read(nameof(DefaultHourlyCharge), 200d); }
			set { Write(nameof(DefaultHourlyCharge), value); }
		}



		private T Read<T>(string key, T defaultValue) => settings.GetValueOrDefault(key, defaultValue);

		private void Write<T>(string key, T value)
		{
			settings.AddOrUpdateValue(key, value);
			changed.OnNext(new KeyValuePair<string, object>(key, value));
		}



		private static SettingsService _current;
		private ISettings settings;
		private Subject<KeyValuePair<string, object>> changed;
	}
}
