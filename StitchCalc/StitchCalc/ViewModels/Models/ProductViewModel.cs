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
		readonly ReactiveCommand<object> toggleChargeForMaterials;
		readonly ReactiveCommand<object> toggleChargeForWork;
		readonly IReactiveDerivedList<ProductMaterialViewModel> productMaterials;
		readonly IReactiveDerivedList<WorkUnitViewModel> workUnits;
		readonly IReactiveDerivedList<CustomPropertyViewModel> customProperties;
		readonly ObservableAsPropertyHelper<double> materialsPrice;
		readonly ObservableAsPropertyHelper<double> workInMinutes;
		readonly ObservableAsPropertyHelper<double> workPrice;
		readonly ObservableAsPropertyHelper<double> totalPrice;
		readonly bool isMaterialsPriceMultiplied;
		readonly bool isWorkPriceMultiplied;

		public ProductViewModel(Product productModel)
		{
			model = productModel;
			productMaterials = DataService.Current.GetProductMaterialsForProduct(model.Id);
			workUnits = DataService.Current.GetWorkUnitsForProduct(model.Id);
			customProperties = DataService.Current.GetCustomPropertiesForProduct(model.Id);
			isMaterialsPriceMultiplied = model.MaterialsMultiplier > 0;
			isWorkPriceMultiplied = model.WorkMultiplier > 0;

			delete = ReactiveCommand.Create();
			delete
				.Subscribe(_ => DataService.Current.Remove(model));

			toggleChargeForMaterials = ReactiveCommand.Create();
			toggleChargeForMaterials
				.Subscribe(_ =>
				{
					var m = CopyProduct(Model);
					m.ChargeForMaterials = !m.ChargeForMaterials;
					DataService.Current.Update(m);
				});

			toggleChargeForWork = ReactiveCommand.Create();
			toggleChargeForWork
				.Subscribe(_ =>
				{
					var m = CopyProduct(Model);
					m.ChargeForWork = !m.ChargeForWork;
					DataService.Current.Update(m);
				});

			productMaterials
				.Changed
				.Select(_ => productMaterials.Sum(x => x.Price))
				.StartWith(productMaterials.Sum(x => x.Price))
				.ToProperty(this, x => x.MaterialsPrice, out materialsPrice);

			workUnits
				.Changed
				.Select(_ => (double)workUnits.Sum(x => x.Minutes))
				.StartWith((double)workUnits.Sum(x => x.Minutes))
				.ToProperty(this, x => x.WorkInMinutes, out workInMinutes);

			workUnits
				.Changed
				.Select(_ => workUnits.Sum(x => x.TotalCharge))
				.StartWith(workUnits.Sum(x => x.TotalCharge))
				.ToProperty(this, x => x.WorkPrice, out workPrice);

			this
				.WhenAnyValue(x => x.MaterialsPrice, y => y.WorkPrice, (x, y) => Tuple.Create(x,y))
				.Select(x => CalculatePrice(x.Item1, x.Item2, model))
				.ToProperty(this, x => x.TotalPrice, out totalPrice);
		}

		public Product Model => model;

		public ReactiveCommand<object> Delete => delete;

		public ReactiveCommand<object> ToggleChargeForMaterials => toggleChargeForMaterials;

		public ReactiveCommand<object> ToggleChargeForWork => toggleChargeForWork;

		public string Name => model.Name;

		public bool ChargeForMaterials => model.ChargeForMaterials;

		public bool ChargeForWork => model.ChargeForWork;

		public double MaterialsMultiplier => model.MaterialsMultiplier;

		public double WorkMultiplier => model.WorkMultiplier;

		public IReactiveDerivedList<ProductMaterialViewModel> Materials => productMaterials;

		public IReactiveDerivedList<WorkUnitViewModel> WorkUnits => workUnits;

		public IReactiveDerivedList<CustomPropertyViewModel> CustomProperties => customProperties;

		public double MaterialsPrice => materialsPrice.Value;

		public double WorkInMinutes => workInMinutes.Value;

		public double WorkPrice => workPrice.Value;

		public double TotalPrice => totalPrice.Value;

		public bool IsMaterialsPriceMultiplied => isMaterialsPriceMultiplied;

		public bool IsWorkPriceMultiplied => isWorkPriceMultiplied;



		double CalculatePrice(double materialsPrice, double workPrice, Product product)
		{
			var sum = 0d;

			if (product.ChargeForMaterials)
			{
				sum += materialsPrice * (product.MaterialsMultiplier > 0 ? product.MaterialsMultiplier : 1);
			}

			if (product.ChargeForWork)
			{
				sum += workPrice * (product.WorkMultiplier > 0 ? product.WorkMultiplier : 1);
			}

			return sum;
		}

		Product CopyProduct(Product product)
		{
			return new Product
			{
				Id = product.Id,
				Name = product.Name,
				ChargeForMaterials = product.ChargeForMaterials,
				ChargeForWork = product.ChargeForWork,
				MaterialsMultiplier = product.MaterialsMultiplier,
				WorkMultiplier = product.WorkMultiplier
			};
		}
	}
}
