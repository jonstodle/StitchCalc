using Newtonsoft.Json;
using StitchCalc.Models;
using StitchCalc.Services.FileServices;
using System;
using System.Collections.Generic;
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
			data[nameof(productMaterials)] = new DataStorageObject<IEnumerable<ProductMaterial>> { TimeStamp = DateTimeOffset.UtcNow, Data = productMaterials };

			await FileService.WriteDataAsync(_dataFileName, data);
		}

		async Task LoadDataFromDisk()
		{
			var data = await FileService.ReadDataAsync<Dictionary<string, object>>(_dataFileName);

			if (data == null) { return; }

			using (products.SuppressChangeNotifications())
			using (materials.SuppressChangeNotifications())
			using (workUnits.SuppressChangeNotifications())
			using (productMaterials.SuppressChangeNotifications())
			{
				var productsDso = JsonConvert.DeserializeObject<DataStorageObject<IEnumerable<Product>>>(data[nameof(products)].ToString());

				if (productsDso != null) { products.AddRange(productsDso.Data); }
			

			
				var materialDso = JsonConvert.DeserializeObject<DataStorageObject<IEnumerable<Material>>>(data[nameof(materials)].ToString());

				if (materialDso != null) { materials.AddRange(materialDso.Data); }
			

			
				var workUnitsDso = JsonConvert.DeserializeObject<DataStorageObject<IEnumerable<WorkUnit>>>(data[nameof(workUnits)].ToString());

				if (workUnitsDso != null) { workUnits.AddRange(workUnitsDso.Data); }
			

			
				var productMaterialsDso = JsonConvert.DeserializeObject<DataStorageObject<IEnumerable<ProductMaterial>>>(data[nameof(productMaterials)].ToString());

				if (productMaterialsDso != null) { productMaterials.AddRange(productMaterialsDso.Data); }
			}
		}
	}
}
