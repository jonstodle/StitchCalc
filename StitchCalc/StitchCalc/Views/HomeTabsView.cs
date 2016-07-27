using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System;
using System.Linq;
using System.Reactive.Linq;

using Xamarin.Forms;

namespace StitchCalc.Views
{
	public class HomeTabsView : TabbedPage, IViewFor<HomeTabsViewViewModel>
	{
		public HomeTabsView ()
		{
			Title = "StitchCalc";

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
				});
		}

		public HomeTabsViewViewModel ViewModel
		{
			get { return (HomeTabsViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(HomeTabsViewViewModel), typeof(HomeTabsView), new HomeTabsViewViewModel());

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (HomeTabsViewViewModel)value; }
		}
	}
}
