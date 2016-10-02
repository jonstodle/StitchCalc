using System;
using System.Reactive.Linq;

namespace StitchCalc.Services.DataServices
{
	public partial class DataService
	{
		public static DataService Current { get; }
		static DataService() { Current = Current ?? new DataService(); }



		DataService()
		{
			Observable.FromAsync(() => LoadDataFromDisk())
				.Take(1)
				.Subscribe(_ => Observable.Merge(
					products.Changed,
					materials.Changed,
					workUnits.Changed,
					productMaterials.Changed)
					.Throttle(TimeSpan.FromSeconds(1))
					.Subscribe(async __ => await SaveDataToDisk()));
		}
	}
}
