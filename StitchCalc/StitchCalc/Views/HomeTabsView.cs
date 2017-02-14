using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System;
using System.Linq;
using System.Reactive.Linq;

using Xamarin.Forms;
using System.Reactive.Disposables;
using System.Reactive;

namespace StitchCalc.Views
{
	public class HomeTabsView : TabbedPage, IViewFor<HomeTabsViewViewModel>, ICanActivate
	{
		public HomeTabsView ()
		{
			ViewModel = new HomeTabsViewViewModel();

			Title = "StitchCalc";

			this.WhenActivated(disposables =>
			{
				ViewModel
					.Pages
					.Changed
					.Throttle(TimeSpan.FromMilliseconds(10))
					.Select(_ => ViewModel.Pages)
					.StartWith(ViewModel.Pages)
					.Subscribe(pages =>
					{
						Children.Clear();
						foreach (var page in pages) { Children.Add(page); }
					})
					.DisposeWith(disposables);
			});
		}

		public HomeTabsViewViewModel ViewModel
		{
			get { return (HomeTabsViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(HomeTabsViewViewModel), typeof(HomeTabsView), null);

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (HomeTabsViewViewModel)value; }
		}

		public IObservable<Unit> Activated => Observable.FromEventPattern(this, nameof(this.Appearing)).ToSignal();

		public IObservable<Unit> Deactivated => Observable.FromEventPattern(this, nameof(this.Disappearing)).ToSignal();
	}
}
