using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StitchCalc.Services.DataServices
{
    public partial class DataService
    {
		readonly ReactiveList<Material> materials = new ReactiveList<Material>();
		public IReactiveDerivedList<MaterialViewModel> GetMaterials() => materials.CreateDerivedCollection(x=>new MaterialViewModel(x));
		public MaterialViewModel GetMaterial(Guid materialId)
		{
			var m = materials.FirstOrDefault(x => x.Id == materialId);
			return m != default(Material) ? new MaterialViewModel(m) : default(MaterialViewModel);
		}



		public Material Add(Material material)
		{
			material.Id = Guid.NewGuid();

			if (string.IsNullOrWhiteSpace(material.Name)) { throw new ArgumentNullException(nameof(material.Name)); }

			if (material.Price == default(double)) { throw new ArgumentNullException(nameof(material.Price)); }

			if (material.Width == default(double)) { throw new ArgumentNullException(nameof(material.Width)); }

			materials.Add(material);

			return material;
		}

		public bool Remove(Material productMaterial)
		{
			var m = materials.FirstOrDefault(x => x.Id == productMaterial.Id);

			if (m == default(Material)) { throw new ArgumentException("Material id not found"); }

			return materials.Remove(m);
		}

		public Material Update(Material material)
		{
			var m = materials.FirstOrDefault(x => x.Id == material.Id);

			if (m == default(Material)) { throw new ArgumentException("Material id not found"); }

			using (materials.SuppressChangeNotifications())
			{
				Remove(m);
				Add(material).Id = m.Id;
			}

			return material;
		}
	}
}
