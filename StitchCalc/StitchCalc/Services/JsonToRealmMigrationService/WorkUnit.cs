using System;

namespace StitchCalc.Services.JsonToRealmMigrationService
{
	public class WorkUnit
    {
		public Guid Id { get; set; }
		public Guid ProductId { get; set; }
		public string Name { get; set; }
		public double Charge { get; set; }
		public int Minutes { get; set; }
	}
}
