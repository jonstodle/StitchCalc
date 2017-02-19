using System;
using System.Collections.Generic;
using System.Text;

namespace StitchCalc.Models
{
    public interface IGuidId
    {
		string StringId { get; set; }
        Guid Id { get; set; }
    }
}
