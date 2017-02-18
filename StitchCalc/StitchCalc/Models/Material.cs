using System;
using Realms;

namespace StitchCalc.Models
{
	public class Material : RealmObject, IGuidId
	{
		[PrimaryKey]
		public string StringId { get; set; } = Guid.NewGuid().ToString();

		public string Name { get; set; }
		public double Price { get; set; }
		public double Width { get; set; }
		public string Notes { get; set; }



		public Guid Id { get { return Guid.Parse(StringId); } set { StringId = value.ToString(); } }
	}
}
