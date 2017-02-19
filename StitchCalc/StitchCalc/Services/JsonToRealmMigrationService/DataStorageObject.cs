using System;

namespace StitchCalc.Services.JsonToRealmMigrationService
{
	public class DataStorageObject<T>
    {
		public DateTimeOffset TimeStamp { get; set; }
		public T Data { get; set; }
	}
}
