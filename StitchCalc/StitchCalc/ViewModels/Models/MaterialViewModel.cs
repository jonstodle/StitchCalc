using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.DataServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace StitchCalc.ViewModels.Models
{
    public class MaterialViewModel : ViewModelBase
    {
		readonly Material model;
		readonly ReactiveCommand<object> delete;

		public MaterialViewModel(Material material)
		{
			model = material;

			delete = ReactiveCommand.Create();
			delete
				.Subscribe(_ => DataService.Current.Remove(model));
		}

		public Material Model => model;

		public ReactiveCommand<object> Delete => delete;

		public string Name => model.Name;

		public string Description => model.Description;

		public double PricePerAmount => model.Price;

		public double Price => model.Price * model.Amount;

		public double Amount => model.Amount;
    }
}
