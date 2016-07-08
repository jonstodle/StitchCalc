using System;
using System.Collections.Generic;
using System.Text;

namespace StitchCalc.Models
{
    public class CustomProperty
    {
		public Guid Id { get; set; }
		public Guid ProductId { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
	}
}
