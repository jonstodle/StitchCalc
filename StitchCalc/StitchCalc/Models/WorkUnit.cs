using System;
using Realms;

namespace StitchCalc.Models
{
	public class WorkUnit : RealmObject
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

		public string Name { get; set; }
		public double Charge { get; set; }
		public int Minutes { get; set; }
	}
}
