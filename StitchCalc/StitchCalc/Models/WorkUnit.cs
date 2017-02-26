using System;
using Realms;

namespace StitchCalc.Models
{
	public class WorkUnit : RealmObject, IRecord
    {
        [PrimaryKey]
		public string StringId { get; set; } = Guid.NewGuid().ToString();

        public Product Product { get; set; }
        public string Name { get; set; }
		public double Charge { get; set; }
		public int Minutes { get; set; }
        public DateTimeOffset Added { get; set; } = DateTimeOffset.Now;



        public Guid Id { get { return Guid.Parse(StringId); } set { StringId = value.ToString(); } }
	}
}
