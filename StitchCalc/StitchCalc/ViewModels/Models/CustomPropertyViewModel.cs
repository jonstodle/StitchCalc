using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.DataServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace StitchCalc.ViewModels.Models
{
    public class CustomPropertyViewModel
    {
		readonly CustomProperty model;
		readonly ReactiveCommand<object> delete;

		public CustomPropertyViewModel(CustomProperty customProperty)
		{
			model = customProperty;

			delete = ReactiveCommand.Create();
			delete
				.Subscribe(_ => DataService.Current.Remove(model));
		}

		public CustomProperty Model => model;

		public ReactiveCommand<object> Delete => delete;

		public string Name => model.Name;
		public string Value => model.Value;
    }
}
