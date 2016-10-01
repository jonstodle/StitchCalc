using System;
using System.Reactive.Linq;

namespace System.Reactive.Linq
{
	public static class SystemReactiveLinqExtensions
	{
		public static IObservable<T> WhereNotNull<T>(this IObservable<T> source) where T : class => source.Where(x => x != null);

		public static IObservable<Unit> ToSignal<T>(this IObservable<T> source) => source.Select(_ => Unit.Default);
	}
}
