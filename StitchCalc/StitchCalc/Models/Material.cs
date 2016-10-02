using System;

namespace StitchCalc.Models
{
	public class Material
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }
		public double Width { get; set; }
		public string Notes { get; set; }
	}
}
