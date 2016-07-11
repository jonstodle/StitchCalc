using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StitchCalc.Services.DataServices
{
    public partial class DataService
    {
		readonly ReactiveList<WorkUnit> workUnits = new ReactiveList<WorkUnit>();
		public IReactiveDerivedList<WorkUnitViewModel> GetWorkUnits() => workUnits.CreateDerivedCollection(x => new WorkUnitViewModel(x));
		public IReactiveDerivedList<WorkUnitViewModel> GetWorkUnitsForProduct(Guid productId) => workUnits.CreateDerivedCollection(x => new WorkUnitViewModel(x), x => x.ProductId == productId);
		public WorkUnitViewModel GetWorkUnit(Guid workUnitId)
		{
			var wu = workUnits.FirstOrDefault(x => x.Id == workUnitId);
			return wu != default(WorkUnit) ? new WorkUnitViewModel(wu) : default(WorkUnitViewModel);
		}



		public WorkUnit Add(WorkUnit workUnit)
		{
			workUnit.Id = Guid.NewGuid();

			if (workUnit.ProductId == default(Guid)) { throw new ArgumentNullException(nameof(workUnit.ProductId)); }

			if (workUnit.Charge == default(double)) { throw new ArgumentNullException(nameof(workUnit.Charge)); }

			if (workUnit.Minutes == default(long)) { throw new ArgumentNullException(nameof(workUnit.Minutes)); }

			workUnits.Add(workUnit);

			return workUnit;
		}

		public bool Remove(WorkUnit workUnit)
		{
			var wu = workUnits.FirstOrDefault(x => x.Id == workUnit.Id);

			if (wu == default(WorkUnit)) { throw new ArgumentException("WorkUnit id not found"); }

			return workUnits.Remove(wu);
		}

		public WorkUnit Update(WorkUnit workUnit)
		{
			var wu = workUnits.FirstOrDefault(x => x.Id == workUnit.Id);

			if (wu == default(WorkUnit)) { throw new ArgumentException("WorkUnit id not found"); }

			using (workUnits.SuppressChangeNotifications())
			{
				Remove(wu);
				Add(workUnit);
			}

			return workUnit;
		}
	}
}
