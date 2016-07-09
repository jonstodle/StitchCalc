using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.FileServices;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StitchCalc.Services.DataServices
{
	public partial class DataService
	{
		public static DataService Current { get; }
		static DataService() { Current = Current ?? new DataService(); }



		DataService()
		{
#pragma warning disable 4014
			products.Changed
				.Merge(materials.Changed)
				.Merge(workUnits.Changed)
				.Subscribe(_ => SaveDataToDisk());
#pragma warning restore 4014
		}
	}
}
