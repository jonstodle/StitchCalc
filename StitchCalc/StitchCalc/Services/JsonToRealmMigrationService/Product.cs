using System;

namespace StitchCalc.Services.JsonToRealmMigrationService
{
	public class Product
    {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public bool ChargeForMaterials { get; set; } = true;
		public bool ChargeForWork { get; set; } = true;
		public double MaterialsMultiplier { get; set; }
		public double WorkMultiplier { get; set; }
	}
}
