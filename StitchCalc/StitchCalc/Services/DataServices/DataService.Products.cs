using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.ViewModels.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StitchCalc.Services.DataServices
{
	public partial class DataService
	{
		readonly ReactiveList<Product> products = new ReactiveList<Product>();
		public IReactiveDerivedList<ProductViewModel> GetProducts() => products.CreateDerivedCollection(x => new ProductViewModel(x));
		public ProductViewModel GetProduct(Guid productId)
		{
			var p = products.FirstOrDefault(x => x.Id == productId);
			return p != default(Product) ? new ProductViewModel(p) : default(ProductViewModel);
		}



		public Product Add(Product product)
		{
			product.Id = Guid.NewGuid();

			if (string.IsNullOrWhiteSpace(product.Name)) { throw new ArgumentNullException(nameof(product.Name)); }

			products.Add(product);

			return product;
		}

		public bool Remove(Product product)
		{
			var p = products.FirstOrDefault(x => x.Id == product.Id);

			if (p == default(Product)) { throw new ArgumentException("Product id not found"); }


			using (products.SuppressChangeNotifications())
			using (workUnits.SuppressChangeNotifications())
			using (productMaterials.SuppressChangeNotifications())
			using (customProperties.SuppressChangeNotifications())
			{
				for (int i = workUnits.Count - 1; i >= 0; i--)
				{
					var item = workUnits[i];

					if (item.ProductId == p.Id) { Remove(item); }
				}

				for (int i = productMaterials.Count - 1; i >= 0; i--)
				{
					var item = productMaterials[i];

					if (item.ProductId == p.Id) { Remove(item); }
				}

				for (int i = customProperties.Count - 1; i >= 0; i--)
				{
					var item = customProperties[i];

					if (item.ParentId == p.Id) { Remove(item); }
				}

				return products.Remove(p);
			}
		}

		public Product Update(Product product)
		{
			var p = products.FirstOrDefault(x => x.Id == product.Id);

			if (p == default(Product)) { throw new ArgumentException("Product id not found"); }

			using (products.SuppressChangeNotifications())
			{
				products.Remove(p);
				Add(product).Id = p.Id;
			}

			return product;
		}
	}
}
