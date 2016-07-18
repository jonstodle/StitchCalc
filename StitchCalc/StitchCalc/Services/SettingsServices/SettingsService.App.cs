using System;
using System.Collections.Generic;
using System.Text;

namespace StitchCalc.Services.SettingsServices
{
    public partial class SettingsService
    {
		public bool IsFirstRun
		{
			get { return Read(nameof(IsFirstRun), true); }
			set { Write(nameof(IsFirstRun), value); }
		}
	}
}
