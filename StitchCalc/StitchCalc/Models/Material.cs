using System;
using Realms;

namespace StitchCalc.Models
{
	public class Material : RealmObject, IRecord
	{
		[PrimaryKey]
		public string StringId { get; set; } = Guid.NewGuid().ToString();

		public string Name { get; set; }
		public double Price { get; set; }
		public double Width { get; set; }
		public string Notes { get; set; }
        public DateTimeOffset Added { get; set; } = DateTimeOffset.Now;



        public Guid Id { get { return Guid.Parse(StringId); } set { StringId = value.ToString(); } }
	}
}
