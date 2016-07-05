using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
#if WINDOWS_UWP
using Windows.Storage;
#endif

namespace StitchCalc.Services.FileServices
{
    public partial class FileService
    {
#if WINDOWS_UWP
		public async static Task<bool> WriteDataAsync<T>(string name, T data)
		{
			try
			{
				var localFolder = ApplicationData.Current.LocalFolder;
				var json = JsonConvert.SerializeObject(data);
				var file = await localFolder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
				await FileIO.WriteTextAsync(file, json);

				return true;
			}
			catch { /* Do nothing */}

			return false;
		}

		public async static Task<T> ReadDataAsync<T>(string name)
		{
			var localFolder = ApplicationData.Current.LocalFolder;
			var file = await localFolder.CreateFileAsync(name, CreationCollisionOption.OpenIfExists);
			var json = await FileIO.ReadTextAsync(file);
			return JsonConvert.DeserializeObject<T>(json);
		}
#endif
	}
}
