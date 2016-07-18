using Newtonsoft.Json;
using StitchCalc.Services.FileServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StitchCalc.Services.SettingsServices
{
	public partial class SettingsService
	{
		public static SettingsService Current { get; }
		static SettingsService() { Current = Current ?? new SettingsService(); }



		SettingsService() { }


		Dictionary<string, object> settings;

		public async Task Init()
		{
			settings = (await FileService.ReadDataAsync<Dictionary<string, object>>($"{nameof(settings)}.json")) ?? new Dictionary<string, object>();
		}

		async Task SaveSettingsToDisk()
		{
			await FileService.WriteDataAsync($"{nameof(settings)}.json", settings);
		}

		T Read<T>(string key, T defaultValue)
		{
			if (settings[key] is T) { return (T)settings[key]; }
			else { return defaultValue; }
		}

		async void Write<T>(string key, T value)
		{
			settings[key] = value;
			await SaveSettingsToDisk();
		}
	}
}
