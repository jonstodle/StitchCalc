using System;
using System.Collections.Generic;
using Realms;

namespace StitchCalc.Models
{
	public class Product : RealmObject
    {
		[PrimaryKey]
		public Guid Id { get; set; }

		public string Name { get; set; }
		public bool ChargeForMaterials { get; set; } = true;
		public bool ChargeForWork { get; set; } = true;
		public double MaterialsMultiplier { get; set; }
		public double WorkMultiplier { get; set; }
		public IList<ProductMaterial> Materials { get; set; }
		public IList<WorkUnit> WorkUnits { get; set; }
	}
}
