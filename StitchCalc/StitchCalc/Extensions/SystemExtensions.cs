namespace System
{
	public static class SystemExtensions
    {
		public static bool IsValidDouble(this string s)
		{
			double d;
			return double.TryParse(s, out d);
		}

        public static bool HasValue(this string source, int minimumLength = 1) => string.IsNullOrWhiteSpace(source) && source.Length >= minimumLength;
	}
}
