using System;
using System.Linq;
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



		public static Realm DB => _realm;



		private static Realm _realm;
	}
}
