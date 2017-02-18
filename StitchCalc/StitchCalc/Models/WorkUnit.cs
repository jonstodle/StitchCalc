using System;
using Realms;

namespace StitchCalc.Models
{
	public class WorkUnit : RealmObject, IGuidId
    {
        [PrimaryKey]
		public string StringId { get; set; } = Guid.NewGuid().ToString();

        public Product Product { get; set; }
        public string Name { get; set; }
		public double Charge { get; set; }
		public int Minutes { get; set; }



		public Guid Id { get { return Guid.Parse(StringId); } set { StringId = value.ToString(); } }



		public double ChargePerHour
        {
            get { return Charge * 60; }
            set { Charge = value / 60; }
        }
	}
}
