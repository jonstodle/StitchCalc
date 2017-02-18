using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StitchCalc.Models;

namespace StitchCalc
{
	public class JsonToRealmMigrationService
	{
		public JsonToRealmMigrationService(Dictionary<string, object> jsonData)
		{
			_jsonData = jsonData;
		}



		public IObservable<IEnumerable<Product>> GetProducts() => Observable.Start(() =>
		{
			var storageObject = JObject.Parse(_jsonData["products"].ToString());
			return JsonConvert.DeserializeObject<IEnumerable<Product>>(storageObject["Data"].ToString());
		});

		public IObservable<IEnumerable<Material>> GetMaterials() => Observable.Start(() =>
		{
			var storageObject = JObject.Parse(_jsonData["materials"].ToString());
			return JsonConvert.DeserializeObject<IEnumerable<Material>>(storageObject["Data"].ToString());
		});

		public IObservable<IEnumerable<WorkUnit>> GetWorkUnits(Product product) => Observable.Start(() =>
		{
			var storageObject = JObject.Parse(_jsonData["workUnits"].ToString());
			return JArray.Parse(storageObject["Data"].ToString())
						 .Where(x => x["ProductId"].ToString() == product.Id.ToString())
						 .Select(x => JsonConvert.DeserializeObject<WorkUnit>(x.ToString()));
		});

		public IObservable<IEnumerable<ProductMaterial>> GetProductMaterials(Product product, IEnumerable<Material> materials) => Observable.Start(() =>
		{
			var storageObject = JObject.Parse(_jsonData["productMaterials"].ToString());
			return JArray.Parse(storageObject["Data"].ToString())
						 .Where(x => x["ProductId"].ToString() == product.Id.ToString())
						 .Select(x =>
						 {
							 var material = materials.FirstOrDefault(y => y.Id.ToString() == x["MaterialId"].ToString());
							 if (material == null) return null;

							 var productMaterial = new ProductMaterial(material);
							 productMaterial.Id = JsonConvert.DeserializeObject<Guid>(x["Id"].ToString());
							 productMaterial.Length = JsonConvert.DeserializeObject<double>(x["Length"].ToString());
							 return productMaterial;
						 })
				         .Where(x => x != null);
		});



		Dictionary<string, object> _jsonData;
	}
}
