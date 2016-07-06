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
		readonly ReactiveList<Product> products = new ReactiveList<Product>();
		public IReadOnlyList<Product> GetProducts() => products;



		public void Add(Product product)
		{
			product.Id = Guid.NewGuid();

			if (string.IsNullOrWhiteSpace(product.Name)) { throw new ArgumentNullException(nameof(product.Name)); }

			if (product.Materials == default(IEnumerable<Material>)) { throw new ArgumentNullException(nameof(product.Materials)); }

			if (product.WorkUnits == default(IEnumerable<WorkUnit>)) { throw new ArgumentNullException(nameof(product.WorkUnits)); }

			products.Add(product);
		}

		public void Remove(Product product)
		{
			var p = products.FirstOrDefault(x => x.Id == product.Id);

			if (p == default(Product)) { throw new ArgumentException("Product id not found"); }

			products.Remove(p);
		}

		public void Update(Product product)
		{
			var p = products.FirstOrDefault(x => x.Id == product.Id);

			if (p == default(Product)) { throw new ArgumentException("Product id not found"); }

			using (products.SuppressChangeNotifications())
			{
				Remove(p);
				Add(product);
			}

			p.Name = product.Name;
		}
	}
}
