using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Realms;
using StitchCalc.Models;
using StitchCalc.Services.JsonToRealmMigrationService;

namespace StitchCalc
{
	public static class DBService
	{
		static DBService()
		{
			var realmConfig = new RealmConfiguration();
#if DEBUG
			realmConfig.ShouldDeleteIfMigrationNeeded = true;
#endif

			_realm = Realm.GetInstance(realmConfig);

			var dataFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "data.json");
			if (File.Exists(dataFilePath)) new MigrationClient(dataFilePath).MigrateData().Subscribe(_ => { }, _ => { }, () => File.Delete(dataFilePath));
		}



		public static void Write(Action<Realm> writeActions) => _realm.Write(() => writeActions(_realm));



        public static T GetSingle<T>(Expression<Func<T, bool>> findFunction) where T : RealmObject => _realm.All<T>().FirstOrDefault(findFunction);

		public static T GetSingle<T>(Guid id) where T : RealmObject, IRecord
		{
			var stringGuid = id.ToString();
			return _realm.All<T>().Where(x => x.StringId == stringGuid).AsRealmCollection().FirstOrDefault();
		}



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
