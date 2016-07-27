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
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			LoadDataFromDisk();

			Observable.Merge(
				products.Changed,
				materials.Changed,
				workUnits.Changed,
				customProperties.Changed,
				productMaterials.Changed)
				.Throttle(TimeSpan.FromSeconds(1))
				.Subscribe(_ => SaveDataToDisk());
#pragma warning restore 4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		}
	}
}
