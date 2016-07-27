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
