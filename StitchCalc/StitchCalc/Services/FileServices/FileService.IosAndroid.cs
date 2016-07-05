using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StitchCalc.Services.FileServices
{
    public partial class FileService
    {
#if !WINDOWS_UWP
		public async static Task<bool> WriteDataAsync<T>(string name, T data)
		{
			try
			{
				var json = JsonConvert.SerializeObject(data);

				var dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				var filePath = Path.Combine(dirPath, name);

				File.WriteAllText(filePath, json);

				return await Task.FromResult(true);
			}
			catch { /* Do nothing */ }

			return await Task.FromResult(false);
		}

		public async static Task<T> ReadDataAsync<T>(string name)
		{
			var dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var filePath = Path.Combine(dirPath, name);

			if (File.Exists(filePath))
			{
				var json = File.ReadAllText(filePath);
				return await Task.FromResult(JsonConvert.DeserializeObject<T>(json));
			}
			else { return await Task.FromResult(default(T)); }
		}
#endif
	}
}
