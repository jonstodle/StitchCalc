using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace System.Collections.Specialized
{
    public static class SystemCollectionsSpecializedExtensions
    {
        public static IObservable<EventPattern<NotifyCollectionChangedEventArgs>> CollectionChanges(this INotifyCollectionChanged source) => Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(source, nameof(source.CollectionChanged));
    }
}
