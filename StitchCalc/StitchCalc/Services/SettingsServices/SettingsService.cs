using Newtonsoft.Json;
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
			settings = new Dictionary<string, object>();
			changed = new Subject<KeyValuePair<string, object>>();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			LoadSettingsFromDisk();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		}

		Dictionary<string, object> settings;
		Subject<KeyValuePair<string, object>> changed;

		public IObservable<KeyValuePair<string, object>> Changed => changed;

		private async Task LoadSettingsFromDisk()
		{
			var loadedSettings = (await FileService.ReadDataAsync<Dictionary<string, object>>($"{nameof(settings)}.json")) ?? new Dictionary<string, object>();

			foreach (var key in loadedSettings.Keys)
			{
				settings[key] = loadedSettings[key];
				changed.OnNext(new KeyValuePair<string, object>(key, settings[key]));
			}
		}

		async Task SaveSettingsToDisk()
		{
			await FileService.WriteDataAsync($"{nameof(settings)}.json", settings);
		}

		T Read<T>(string key, T defaultValue)
		{
			if (!settings.ContainsKey(key)) { return defaultValue; }
			if (!(settings[key] is T)) { return defaultValue; }
			return (T)settings[key];
		}

		async void Write<T>(string key, T value)
		{
			if (!value.Equals(Read(key, default(T))))
			{
				settings[key] = value;
				await SaveSettingsToDisk();
				changed.OnNext(new KeyValuePair<string, object>(key, value)); 
			}
		}
	}
}
