using StitchCalc.Models;
using StitchCalc.Services.FileServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StitchCalc.Services.DataServices
{
	public partial class DataService
	{
		const string _dataFileName = "data.json";

		async Task SaveDataToDisk()
		{
			var data = new Dictionary<string, object>();

			data[nameof(products)] = new DataStorageObject<IEnumerable<Product>> { TimeStamp = DateTimeOffset.UtcNow, Data = products };
			data[nameof(materials)] = new DataStorageObject<IEnumerable<Material>> { TimeStamp = DateTimeOffset.UtcNow, Data = materials };
			data[nameof(workUnits)] = new DataStorageObject<IEnumerable<WorkUnit>> { TimeStamp = DateTimeOffset.UtcNow, Data = workUnits };

			await FileService.WriteDataAsync(_dataFileName, data);
		}

		async Task LoadDataFromDisk()
		{
			var data = await FileService.ReadDataAsync<Dictionary<string, object>>(_dataFileName);

			if (data == null) { return; }

			using (products.SuppressChangeNotifications())
			{
				var productsDso = data[nameof(products)] as DataStorageObject<IEnumerable<Product>>;

				if (productsDso != null) { products.AddRange(productsDso.Data); }
			}

			using (materials.SuppressChangeNotifications())
			{
				var materialDso = data[nameof(materials)] as DataStorageObject<IEnumerable<Material>>;

				if (materialDso != null) { materials.AddRange(materialDso.Data); }
			}

			using (workUnits.SuppressChangeNotifications())
			{
				var workUnitsDso = data[nameof(workUnits)] as DataStorageObject<IEnumerable<WorkUnit>>;

				if (workUnitsDso != null) { workUnits.AddRange(workUnitsDso.Data); }
			}
		}
	}
}
