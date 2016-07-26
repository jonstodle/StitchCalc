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
		readonly IReactiveDerivedList<CustomPropertyViewModel> customProperties;

		public MaterialViewModel(Material material)
		{
			model = material;
			customProperties = DataService.Current.GetCustomPropertiesForParent(model.Id);

			delete = ReactiveCommand.Create();
			delete
				.Subscribe(_ => DataService.Current.Remove(model));
		}

		public ReactiveCommand<object> Delete => delete;

		public Material Model => model;

		public IReactiveDerivedList<CustomPropertyViewModel> CustomProperties => customProperties;

		public string Name => model.Name;

		public double PricePerMeter => model.Price;

		public double Width => model.Width;
    }
}
