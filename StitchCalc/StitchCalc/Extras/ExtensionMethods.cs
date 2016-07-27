namespace StitchCalc.Extras
{
	public static class ExtensionMethods
    {
		public static bool IsValidDouble(this string s)
		{
			double d;
			return double.TryParse(s, out d);
		}
    }
}
