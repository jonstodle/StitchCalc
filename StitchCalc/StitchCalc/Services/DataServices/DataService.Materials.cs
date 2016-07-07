using ReactiveUI;
using StitchCalc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StitchCalc.Services.DataServices
{
    public partial class DataService
    {
		readonly ReactiveList<Material> materials = new ReactiveList<Material>();
		public IReadOnlyList<Material> GetMaterials() => materials;
		public IReadOnlyList<Material> GetMaterials(Guid productId) => materials.CreateDerivedCollection(x => x, x => x.ProductId == productId);



		public void Add(Material material)
		{
			material.Id = Guid.NewGuid();

			if (material.ProductId == default(Guid)) { throw new ArgumentNullException(nameof(material.ProductId)); }

			if (string.IsNullOrWhiteSpace(material.Name)) { throw new ArgumentNullException(nameof(material.Name)); }

			if (material.Price == default(decimal)) { throw new ArgumentNullException(nameof(material.Price)); }

			if (material.Amount == default(double)) { throw new ArgumentNullException(nameof(material.Amount)); }

			materials.Add(material);
		}

		public void Remove(Material material)
		{
			var m = materials.FirstOrDefault(x => x.Id == material.Id);

			if (m == default(Material)) { throw new ArgumentException("Material id not found"); }

			products.Remove(m);
		}

		public void Update(Material material)
		{
			var m = materials.FirstOrDefault(x => x.Id == material.Id);

			if (m == default(Material)) { throw new ArgumentException("Material id not found"); }

			using (materials.SuppressChangeNotifications())
			{
				Remove(m);
				Add(material);
			}
		}
	}
}
