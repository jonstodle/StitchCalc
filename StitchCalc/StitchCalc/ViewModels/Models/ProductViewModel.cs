using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace StitchCalc.ViewModels.Models
{
    public class ProductViewModel : ViewModelBase
    {
		readonly Product model;
		readonly ReactiveCommand<object> delete;
		readonly IReactiveDerivedList<ProductMaterialViewModel> productMaterials;
		readonly IReactiveDerivedList<WorkUnitViewModel> workUnits;
		readonly IReactiveDerivedList<CustomPropertyViewModel> customProperties;
		readonly ObservableAsPropertyHelper<double> materialsPrice;
		readonly ObservableAsPropertyHelper<double> workInMinutes;
		readonly ObservableAsPropertyHelper<double> workPrice;
		readonly ObservableAsPropertyHelper<double> totalPrice;

		public ProductViewModel(Product productModel)
		{
			model = productModel;
			productMaterials = DataService.Current.GetProductMaterialsForProduct(model.Id);
			workUnits = DataService.Current.GetWorkUnitsForProduct(model.Id);
			customProperties = DataService.Current.GetCustomPropertiesForProduct(model.Id);

			delete = ReactiveCommand.Create();
			delete
				.Subscribe(_ => DataService.Current.Remove(model));

			productMaterials
				.Changed
				.Select(_ => productMaterials.Sum(x => x.Price))
				.StartWith(productMaterials.Sum(x => x.Price))
				.ToProperty(this, x => x.MaterialsPrice, out materialsPrice);

			workUnits
				.Changed
				.Select(_ => (double)workUnits.Sum(x => x.Minutes))
				.StartWith((double)workUnits.Sum(x=> x.Minutes))
				.ToProperty(this, x => x.WorkInMinutes, out workInMinutes);

			workUnits
				.Changed
				.Select(_ => workUnits.Sum(x => x.TotalCharge))
				.StartWith(workUnits.Sum(x => x.TotalCharge))
				.ToProperty(this, x => x.WorkPrice, out workPrice);

			this
				.WhenAnyValue(x => x.MaterialsPrice, y => y.WorkPrice, (x, y) => x + y)
				.ToProperty(this, x => x.TotalPrice, out totalPrice);
		}

		public Product Model => model;

		public ReactiveCommand<object> Delete => delete;

		public string Name => model.Name;

		public IReactiveDerivedList<ProductMaterialViewModel> Materials => productMaterials;

		public IReactiveDerivedList<WorkUnitViewModel> WorkUnits => workUnits;

		public IReactiveDerivedList<CustomPropertyViewModel> CustomProperties => customProperties;

		public double MaterialsPrice => materialsPrice.Value;

		public double WorkInMinutes => workInMinutes.Value;

		public double WorkPrice => workPrice.Value;

		public double TotalPrice => totalPrice.Value;
	}
}
