using System;
using System.Collections.Generic;
using System.Text;

namespace StitchCalc.Models
{
    public class WorkUnit
    {
		public Guid Id { get; set; }
		public Guid ProductId { get; set; }
		public decimal Charge { get; set; }
		public long Minutes { get; set; }
	}
}
