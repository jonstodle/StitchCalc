using System;
using StitchCalc.Models;

namespace StitchCalc.ViewModels
{
	public class MaterialViewModel : ViewModelBase
	{
		public MaterialViewModel(Material material)
		{
			_material = material;
		}



		public Material Material => _material;



		private readonly Material _material;
	}
}
