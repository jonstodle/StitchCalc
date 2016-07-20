using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StitchCalc.Services.FileServices;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace StitchCalc.Services.SettingsServices
{
	public partial class SettingsService
	{
		public static SettingsService Current { get; }
		static SettingsService() { Current = Current ?? new SettingsService(); }



		SettingsService()
		{
			settings = CrossSettings.Current;
			changed = new Subject<KeyValuePair<string, object>>();
		}

		ISettings settings;
		Subject<KeyValuePair<string, object>> changed;

		public IObservable<KeyValuePair<string, object>> Changed => changed;

		T Read<T>(string key, T defaultValue) => settings.GetValueOrDefault(key, defaultValue);

		void Write<T>(string key, T value)
		{
			settings.AddOrUpdateValue(key, value);
			changed.OnNext(new KeyValuePair<string, object>(key, value));
		}
	}
}
