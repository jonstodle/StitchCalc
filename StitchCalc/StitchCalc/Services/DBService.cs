using System;
using System.Linq;
using System.Linq.Expressions;
using Realms;
using StitchCalc.Models;

namespace StitchCalc
{
	public static class DBService
	{
		static DBService()
		{
			_realm = Realm.GetInstance();
		}



		public static void Write(Action<Realm> writeActions) => _realm.Write(() => writeActions(_realm));

		public static IRealmCollection<T> Get<T>() where T : RealmObject => _realm.All<T>().AsRealmCollection();

		public static IRealmCollection<T> Get<T, TKey>(Expression<Func<T, TKey>> orderFunction, bool sortAscending = true) where T : RealmObject
		{
			if (sortAscending) return _realm.All<T>().OrderBy(orderFunction).AsRealmCollection();
			else return _realm.All<T>().OrderByDescending(orderFunction).AsRealmCollection();
		}



		private static Realm _realm;
	}
}
