using System;

namespace StitchCalc.Services.JsonToRealmMigrationService
{
	public class ProductMaterial
    {
		public Guid Id { get; set; }
		public Guid ProductId { get; set; }
		public Guid MaterialId { get; set; }
		public double Length { get; set; }
	}
}
