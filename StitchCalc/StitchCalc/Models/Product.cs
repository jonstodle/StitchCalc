using System;
using System.Collections.Generic;
using System.Text;

namespace StitchCalc.Models
{
    public class Product
    {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public IEnumerable<Material> Materials { get; set; } = new List<Material>();
		public IEnumerable<WorkUnit> WorkUnits { get; set; } = new List<WorkUnit>();
	}
}
