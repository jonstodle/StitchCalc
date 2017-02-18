using System;
using System.Collections.Generic;
using Realms;
using System.Linq;

namespace StitchCalc.Models
{
	public class Product : RealmObject, IGuidId
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

		public string Name { get; set; }
		public bool ChargeForMaterials { get; set; } = true;
		public bool ChargeForWork { get; set; } = true;
		public double MaterialsMultiplier { get; set; }
		public double WorkMultiplier { get; set; }
        [Backlink(nameof(ProductMaterial.Product))]
		public IQueryable<ProductMaterial> Materials { get; set; }
        [Backlink(nameof(WorkUnit.Product))]
		public IQueryable<WorkUnit> WorkUnits { get; set; }
	}
}
