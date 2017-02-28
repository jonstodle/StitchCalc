using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reactive.Disposables;

namespace ReactiveUI
{
	public static class ReactiveUIExtensions
	{
		public static IEnumerable<T> ToReactiveObservableList<T>(this IReactiveCollection<T> source) => new ReactiveObservableList<T>(source);
	}

	public class ReactiveObservableList<T> : INotifyCollectionChanged, IEnumerable<T>, IDisposable
	{
		public ReactiveObservableList(IReactiveCollection<T> sourceCollection)
		{
			_collection = sourceCollection ?? new ReactiveList<T>();

			_collection.Changed.Subscribe(args => CollectionChanged?.Invoke(this, args)).DisposeWith(_disposables);
		}



		public event NotifyCollectionChangedEventHandler CollectionChanged;



		public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();

		public void Dispose() => _disposables.Dispose();



		private IReactiveCollection<T> _collection;
		private CompositeDisposable _disposables = new CompositeDisposable();
	}
}
