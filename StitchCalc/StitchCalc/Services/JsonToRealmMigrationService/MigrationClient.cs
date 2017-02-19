using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactiveUI;

namespace StitchCalc.Services.JsonToRealmMigrationService
{
	public class MigrationClient
	{
		public MigrationClient(string dataFilePath)
		{
			if (File.Exists(dataFilePath))
			{
				var json = File.ReadAllText(dataFilePath);
				_jsonData = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
			}
		}



		public IObservable<Unit> MigrateData() => Observable.Start(() =>
		{
			var products = JsonConvert.DeserializeObject<DataStorageObject<IEnumerable<Product>>>(_jsonData["products"].ToString()).Data
									  .Select(product => new Models.Product
									  {
										  Id = product.Id,
										  Name = product.Name,
										  ChargeForMaterials = product.ChargeForMaterials,
										  ChargeForWork = product.ChargeForWork,
										  MaterialsMultiplier = product.MaterialsMultiplier,
										  WorkMultiplier = product.WorkMultiplier
									  });

			var materials = JsonConvert.DeserializeObject<DataStorageObject<IEnumerable<Material>>>(_jsonData["materials"].ToString()).Data
									   .Select(material => new Models.Material
									   {
										   Id = material.Id,
										   Name = material.Name,
										   Price = material.Price,
										   Width = material.Width,
										   Notes = material.Notes
									   });

			var workUnits = JsonConvert.DeserializeObject<DataStorageObject<IEnumerable<WorkUnit>>>(_jsonData["workUnits"].ToString()).Data
									   .Select(workUnit => new Models.WorkUnit
									   {
										   Id = workUnit.Id,
										   Name = workUnit.Name,
										   Charge = workUnit.Charge,
										   Minutes = workUnit.Minutes,
										   Product = products.FirstOrDefault(x => x.Id == workUnit.ProductId)
									   })
									   .Where(workUnit => workUnit.Product != null);

			var productMaterials = JsonConvert.DeserializeObject<DataStorageObject<IEnumerable<ProductMaterial>>>(_jsonData["productMaterials"].ToString()).Data
											  .Select(productMaterial => Tuple.Create(productMaterial, materials.FirstOrDefault(x => x.Id == productMaterial.MaterialId)))
											  .Where(x => x.Item2 != null)
											  .Select(productMaterialAndMaterial => new Models.ProductMaterial(productMaterialAndMaterial.Item2)
											  {
												  Id = productMaterialAndMaterial.Item1.Id,
												  Product = products.FirstOrDefault(x => x.Id == productMaterialAndMaterial.Item1.ProductId),
												  Length = productMaterialAndMaterial.Item1.Length
											  })
											  .Where(productMaterial => productMaterial.Product != null);

			return Tuple.Create(products, materials, workUnits, productMaterials);
		})
		.ObserveOn(RxApp.MainThreadScheduler)
		.Do(data =>
		{
			DBService.Write(realm =>
			{
				foreach (var product in data.Item1) realm.Add(product, true);
				foreach (var material in data.Item2) realm.Add(material, true);
				foreach (var workUnit in data.Item3) realm.Add(workUnit, true);
				foreach (var productMaterial in data.Item4) realm.Add(productMaterial, true);
			});
		})
		.ToSignal();



		Dictionary<string, object> _jsonData;
	}
}
