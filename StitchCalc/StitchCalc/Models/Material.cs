using System;
using Realms;

namespace StitchCalc.Models
{
	public class Material : RealmObject, IGuidId
	{
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

		public string Name { get; set; }
		public double Price { get; set; }
		public double Width { get; set; }
		public string Notes { get; set; }
	}
}
