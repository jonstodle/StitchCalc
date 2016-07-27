using System;

namespace StitchCalc.Models
{
	public class DataStorageObject<T>
    {
		public DateTimeOffset TimeStamp { get; set; }
		public T Data { get; set; }
	}
}
