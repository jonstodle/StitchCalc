namespace StitchCalc.Services.SettingsServices
{
	public partial class SettingsService
    {
		public double DefaultHourlyCharge
		{
			get { return Read(nameof(DefaultHourlyCharge), 200d); }
			set { Write(nameof(DefaultHourlyCharge), value); }
		}
	}
}
