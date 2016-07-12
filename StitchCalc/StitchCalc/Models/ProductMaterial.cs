using System;
using System.Collections.Generic;
using System.Text;

namespace StitchCalc.Models
{
    public class ProductMaterial
    {
		public Guid Id { get; set; }
		public Guid ProductId { get; set; }
		public Guid MaterialId { get; set; }
		public double Length { get; set; }
	}
}
