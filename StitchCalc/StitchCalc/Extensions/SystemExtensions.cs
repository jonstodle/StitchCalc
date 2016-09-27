namespace System
{
	public static class SystemExtensions
    {
		public static bool IsValidDouble(this string s)
		{
			double d;
			return double.TryParse(s, out d);
		}
	}
}
