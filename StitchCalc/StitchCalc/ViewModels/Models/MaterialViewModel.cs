using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.DataServices;
using System;
using System.Reactive;

namespace StitchCalc.ViewModels.Models
{
	public class MaterialViewModel : ViewModelBase
    {
		readonly Material model;
		readonly ReactiveCommand<Unit, Unit> delete;

		public MaterialViewModel(Material material)
		{
			model = material;

			delete = ReactiveCommand.Create(() => { DataService.Current.Remove(model); });
		}

		public ReactiveCommand Delete => delete;

		public Material Model => model;

		public string Name => model.Name;

		public double PricePerMeter => model.Price;

		public double Width => model.Width;
    }
}
