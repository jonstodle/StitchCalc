using System;
using System.Collections.Generic;
using System.Text;

namespace StitchCalc.Models
{
    public interface IRecord
    {
		string StringId { get; set; }
        Guid Id { get; set; }
        string Name { get; set; }
        DateTimeOffset Added { get; set; }
    }
}
