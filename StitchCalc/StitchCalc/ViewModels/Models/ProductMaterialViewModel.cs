﻿using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.DataServices;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace StitchCalc.ViewModels.Models
{
	public class ProductMaterialViewModel : ModelViewModelBase
    {
		readonly ProductMaterial model;
		readonly ReactiveCommand<Unit, Unit> delete;
		readonly IReactiveDerivedList<MaterialViewModel> materials;
		ObservableAsPropertyHelper<MaterialViewModel> material;
		ObservableAsPropertyHelper<string> name;
		ObservableAsPropertyHelper<double> pricePerMeter;
		ObservableAsPropertyHelper<double> price;

		public ProductMaterialViewModel(ProductMaterial productMaterial)
		{
			model = productMaterial;
			materials = DataService.Current.GetMaterials();

			delete = ReactiveCommand.Create(() => { DataService.Current.Remove(model); });

			materials
				.Changed
				.Select(_ => DataService.Current.GetMaterial(model.MaterialId))
				.StartWith(DataService.Current.GetMaterial(model.MaterialId))
				.Where(x => x != null)
				.ToProperty(this, x => x.Material, out material);
			var materialChanged = this
				.WhenAnyValue(x => x.Material)
				.Where(x => x != null);
			materialChanged
				.Select(x => x.Name)
				.ToProperty(this, x => x.Name, out name);
			materialChanged
				.Select(x => x.PricePerMeter)
				.ToProperty(this, x => x.PricePerMeter, out pricePerMeter);
			materialChanged
				.Select(x => (model.Length * x.PricePerMeter))
				.ToProperty(this, x => x.Price, out price);
		}

		public ProductMaterial Model => model;

		public ReactiveCommand Delete => delete;

		public MaterialViewModel Material => material.Value;

		public string Name => name.Value;

		public double Length => model.Length;

		public double PricePerMeter => pricePerMeter.Value;

		public double Price => price.Value;
    }
}
