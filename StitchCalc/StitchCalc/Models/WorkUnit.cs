using System;
using System.Collections.Generic;
using System.Text;

namespace StitchCalc.Models
{
    public class WorkUnit
    {
		public double Charge { get; set; }
		public long Minutes { get; set; }
		public TimeSpan MinutesTimeSpan => TimeSpan.Zero + TimeSpan.FromMinutes(Minutes);
	}
}
