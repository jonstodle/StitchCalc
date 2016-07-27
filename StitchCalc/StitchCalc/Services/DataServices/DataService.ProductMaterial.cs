using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.ViewModels.Models;
using System;
using System.Linq;

namespace StitchCalc.Services.DataServices
{
	public partial class DataService
    {
		readonly ReactiveList<ProductMaterial> productMaterials = new ReactiveList<ProductMaterial>();
		public IReactiveDerivedList<ProductMaterialViewModel> GetProductMaterials() => productMaterials.CreateDerivedCollection(x => new ProductMaterialViewModel(x));
		public IReactiveDerivedList<ProductMaterialViewModel> GetProductMaterialsForProduct(Guid productId) => productMaterials.CreateDerivedCollection(x => new ProductMaterialViewModel(x), x => x.ProductId == productId);
		public ProductMaterialViewModel GetProductMaterial(Guid productMaterialId)
		{
			var pm = productMaterials.FirstOrDefault(x => x.Id == productMaterialId);
			return pm != default(ProductMaterial) ? new ProductMaterialViewModel(pm) : default(ProductMaterialViewModel);
		}



		public ProductMaterial Add(ProductMaterial productMaterial)
		{
			productMaterial.Id = Guid.NewGuid();

			if (productMaterial.ProductId == default(Guid)) { throw new ArgumentNullException(nameof(productMaterial.ProductId)); }

			if (productMaterial.MaterialId == default(Guid)) { throw new ArgumentNullException(nameof(productMaterial.MaterialId)); }

			if (productMaterial.Length == default(double)) { throw new ArgumentNullException(nameof(productMaterial.Length)); }

			productMaterials.Add(productMaterial);

			return productMaterial;
		}

		public bool Remove(ProductMaterial productMaterial)
		{
			var pm = productMaterials.FirstOrDefault(x => x.Id == productMaterial.Id);

			if (pm == default(ProductMaterial)) { throw new ArgumentException("ProductMaterial id not found"); }

			return productMaterials.Remove(pm);
		}

		public ProductMaterial Update(ProductMaterial productMaterial)
		{
			var pm = productMaterials.FirstOrDefault(x => x.Id == productMaterial.Id);

			if (pm == default(ProductMaterial)) { throw new ArgumentException("ProductMaterial id not found"); }

			using (productMaterials.SuppressChangeNotifications())
			{
				Remove(pm);
				Add(productMaterial).Id = pm.Id;
			}

			return productMaterial;
		}
	}
}
