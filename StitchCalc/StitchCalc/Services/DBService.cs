using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Realms;
using StitchCalc.Models;

namespace StitchCalc
{
	public static class DBService
	{
		static DBService()
		{
			_realm = Realm.GetInstance();

			var dataFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "data.json");
			if (File.Exists(dataFilePath))
			{
				var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(dataFilePath));
				new JsonToRealmMigrationService(data).MigrateData().Subscribe();
			}
		}



		public static void Write(Action<Realm> writeActions) => _realm.Write(() => writeActions(_realm));



        public static T GetSingle<T>(Expression<Func<T, bool>> findFunction) where T : RealmObject => _realm.All<T>().FirstOrDefault(findFunction);

		public static T GetSingle<T>(Guid id) where T : RealmObject, IGuidId => _realm.All<T>().AsRealmCollection().FirstOrDefault(x => x.Id == id);



        public static IRealmCollection<T> GetList<T>() where T : RealmObject => _realm.All<T>().AsRealmCollection();

		public static IRealmCollection<T> GetOrderedList<T, TSortKey>(Expression<Func<T, TSortKey>> orderFunction, bool sortAscending = true) where T : RealmObject
		{
			if (sortAscending) return _realm.All<T>().OrderBy(orderFunction).AsRealmCollection();
			else return _realm.All<T>().OrderByDescending(orderFunction).AsRealmCollection();
		}

        public static IRealmCollection<T> GetFilteredList<T, TSortKey>(Expression<Func<T, bool>> filterFunction, Expression<Func<T, TSortKey>> orderFunction = null, bool sortAscending = true) where T: RealmObject
        {
            var records = _realm.All<T>().Where(filterFunction);
            if (orderFunction != null) records = sortAscending ? records.OrderBy(orderFunction) : records.OrderByDescending(orderFunction);
            return records.AsRealmCollection();
        }



		private static Realm _realm;
	}
}
