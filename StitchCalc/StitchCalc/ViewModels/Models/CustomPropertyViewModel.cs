using StitchCalc.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StitchCalc.ViewModels.Models
{
    public class CustomPropertyViewModel
    {
		CustomProperty model;

		public CustomPropertyViewModel(CustomProperty customProperty)
		{
			model = customProperty;
		}

		public string Name => model.Name;
		public string Value => model.Value;
    }
}
