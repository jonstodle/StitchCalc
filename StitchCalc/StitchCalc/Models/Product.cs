using System;
using Realms;
using System.Linq;

namespace StitchCalc.Models
{
	public class Product : RealmObject, IRecord
    {
        [PrimaryKey]
		public string StringId { get; set; } = Guid.NewGuid().ToString();

		public string Name { get; set; }
		public bool ChargeForMaterials { get; set; } = true;
		public bool ChargeForWork { get; set; } = true;
		public double MaterialsMultiplier { get; set; }
		public double WorkMultiplier { get; set; }
        public DateTimeOffset Added { get; set; } = DateTimeOffset.Now;
        [Backlink(nameof(ProductMaterial.Product))]
        public IQueryable<ProductMaterial> Materials { get; }
        [Backlink(nameof(WorkUnit.Product))]
		public IQueryable<WorkUnit> WorkUnits { get; }



		public Guid Id { get { return Guid.Parse(StringId); } set { StringId = value.ToString(); } }
	}
}
