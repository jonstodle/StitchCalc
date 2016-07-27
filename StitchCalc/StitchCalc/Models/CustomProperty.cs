using System;

namespace StitchCalc.Models
{
	public class CustomProperty
    {
		public Guid Id { get; set; }
		public Guid ParentId { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
	}
}
