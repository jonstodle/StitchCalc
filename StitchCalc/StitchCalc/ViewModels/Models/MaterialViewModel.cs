using StitchCalc.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StitchCalc.ViewModels.Models
{
    public class MaterialViewModel : ViewModelBase
    {
		Material model;

		public MaterialViewModel(Material model)
		{
			this.model = model;
		}

		public Material Model => model;

		public string Name => model.Name;

		public double PricePerAmount => model.Price;

		public double Price => model.Price * model.Amount;

		public double Amount => model.Amount;
    }
}
