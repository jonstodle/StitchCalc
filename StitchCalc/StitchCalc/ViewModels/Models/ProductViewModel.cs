﻿using ReactiveUI;
using StitchCalc.Extras;
using StitchCalc.Models;
using StitchCalc.Services.DataServices;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace StitchCalc.ViewModels.Models
{
	public class ProductViewModel : ViewModelBase
	{
		readonly Product model;
		readonly ReactiveCommand<object> delete;
		readonly ReactiveCommand<object> toggleChargeForMaterials;
		readonly ReactiveCommand<object> toggleChargeForWork;
		readonly ReactiveCommand<object> setMaterialsMultiplier;
		readonly ReactiveCommand<object> setWorkMultiplier;
		readonly ObservableAsPropertyHelper<IReactiveDerivedList<ProductMaterialViewModel>> productMaterials;
		readonly ObservableAsPropertyHelper<IReactiveDerivedList<WorkUnitViewModel>> workUnits;
		readonly ObservableAsPropertyHelper<IReactiveDerivedList<CustomPropertyViewModel>> customProperties;
		readonly ObservableAsPropertyHelper<double> materialsPrice;
		readonly ObservableAsPropertyHelper<double> workInMinutes;
		readonly ObservableAsPropertyHelper<double> workPrice;
		readonly ObservableAsPropertyHelper<double> totalPrice;
		readonly bool isMaterialsPriceMultiplied;
		readonly bool isWorkPriceMultiplied;

		public ProductViewModel(Product productModel)
		{
			model = productModel;
			isMaterialsPriceMultiplied = model.MaterialsMultiplier > 0;
			isWorkPriceMultiplied = model.WorkMultiplier > 0;

			DataService.Current.GetProductMaterials()
				.Changed
				.Select(_ => DataService.Current.GetProductMaterialsForProduct(model.Id))
				.StartWith(DataService.Current.GetProductMaterialsForProduct(model.Id))
				.ToProperty(this, x => x.Materials, out productMaterials);

			DataService.Current.GetWorkUnits()
				.Changed
				.Select(_ => DataService.Current.GetWorkUnitsForProduct(model.Id))
				.StartWith(DataService.Current.GetWorkUnitsForProduct(model.Id))
				.ToProperty(this, x => x.WorkUnits, out workUnits);

			DataService.Current.GetCustomProperties()
				.Changed
				.Select(_ => DataService.Current.GetCustomPropertiesForParent(model.Id))
				.StartWith(DataService.Current.GetCustomPropertiesForParent(model.Id))
				.ToProperty(this, x => x.CustomProperties, out customProperties);

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

			setMaterialsMultiplier = ReactiveCommand.Create();
			setMaterialsMultiplier
				.Cast<string>()
				.Where(x => x.IsValidDouble())
				.Select(x => double.Parse(x))
				.Where(x => x >= 0)
				.Subscribe(x =>
				{
					var m = CopyProduct(Model);
					m.MaterialsMultiplier = x;
					DataService.Current.Update(m);
				});

			setWorkMultiplier = ReactiveCommand.Create();
			setWorkMultiplier
				.Cast<string>()
				.Where(x => x.IsValidDouble())
				.Select(x => double.Parse(x))
				.Where(x => x >= 0)
				.Subscribe(x =>
				{
					var m = CopyProduct(Model);
					m.WorkMultiplier = x;
					DataService.Current.Update(m);
				});

			this
				.WhenAnyValue(x => x.Materials)
				.Select(_ => Materials.Sum(x => x.Price))
				.StartWith(Materials.Sum(x => x.Price))
				.ToProperty(this, x => x.MaterialsPrice, out materialsPrice);

			this
				.WhenAnyValue(x => x.WorkUnits)
				.Select(_ => (double)WorkUnits.Sum(x => x.Minutes))
				.StartWith((double)WorkUnits.Sum(x => x.Minutes))
				.ToProperty(this, x => x.WorkInMinutes, out workInMinutes);

			this
				.WhenAnyValue(x => x.WorkUnits)
				.Select(_ => WorkUnits.Sum(x => x.TotalCharge))
				.StartWith(WorkUnits.Sum(x => x.TotalCharge))
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

		public ReactiveCommand<object> SetMaterialsMultiplier => setMaterialsMultiplier;

		public ReactiveCommand<object> SetWorkMultiplier => setWorkMultiplier;

		public string Name => model.Name;

		public bool ChargeForMaterials => model.ChargeForMaterials;

		public bool ChargeForWork => model.ChargeForWork;

		public double MaterialsMultiplier => model.MaterialsMultiplier;

		public double WorkMultiplier => model.WorkMultiplier;

		public IReactiveDerivedList<ProductMaterialViewModel> Materials => productMaterials.Value;

		public IReactiveDerivedList<WorkUnitViewModel> WorkUnits => workUnits.Value;

		public IReactiveDerivedList<CustomPropertyViewModel> CustomProperties => customProperties.Value;

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
