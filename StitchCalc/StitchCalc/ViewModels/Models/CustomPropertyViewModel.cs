using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.DataServices;
using System;
using System.Reactive;

namespace StitchCalc.ViewModels.Models
{
	public class CustomPropertyViewModel
    {
		readonly CustomProperty model;
		readonly ReactiveCommand<Unit, Unit> delete;

		public CustomPropertyViewModel(CustomProperty customProperty)
		{
			model = customProperty;

			delete = ReactiveCommand.Create(() => { DataService.Current.Remove(model); });
		}

		public CustomProperty Model => model;

		public ReactiveCommand Delete => delete;

		public string Name => model.Name;
		public string Value => model.Value;
    }
}
