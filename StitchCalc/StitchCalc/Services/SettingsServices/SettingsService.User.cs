using System;
using System.Collections.Generic;
using System.Text;

namespace StitchCalc.Services.SettingsServices
{
    public partial class SettingsService
    {
		public double DefaultHourlyCharge
		{
			get { return Read(nameof(DefaultHourlyCharge), 200); }
			set { Write(nameof(DefaultHourlyCharge), value); }
		}
	}
}
