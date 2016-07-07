using ReactiveUI;
using StitchCalc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StitchCalc.Services.DataServices
{
    public partial class DataService
    {
		readonly ReactiveList<WorkUnit> workUnits = new ReactiveList<WorkUnit>();
		public IReadOnlyList<WorkUnit> GetWorkUnits() => workUnits;
		public IReadOnlyList<WorkUnit> GetWorkUnits(Guid productId) => workUnits.CreateDerivedCollection(x => x, x => x.ProductId == productId);



		public void Add(WorkUnit workUnit)
		{
			workUnit.Id = Guid.NewGuid();

			if (workUnit.ProductId == default(Guid)) { throw new ArgumentNullException(nameof(workUnit.ProductId)); }

			if (workUnit.Charge == default(decimal)) { throw new ArgumentNullException(nameof(workUnit.Charge)); }

			if (workUnit.Minutes == default(long)) { throw new ArgumentNullException(nameof(workUnit.Minutes)); }

			workUnits.Add(workUnit);
		}

		public void Remove(WorkUnit workUnit)
		{
			var wu = workUnits.FirstOrDefault(x => x.Id == workUnit.Id);

			if (wu == default(WorkUnit)) { throw new ArgumentException("WorkUnit id not found"); }

			workUnits.Remove(wu);
		}

		public void Update(WorkUnit workUnit)
		{
			var wu = workUnits.FirstOrDefault(x => x.Id == workUnit.Id);

			if (wu == default(WorkUnit)) { throw new ArgumentException("WorkUnit id not found"); }

			using (workUnits.SuppressChangeNotifications())
			{
				Remove(wu);
				Add(workUnit);
			}
		}
	}
}
