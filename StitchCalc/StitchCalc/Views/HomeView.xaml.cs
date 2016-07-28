using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace StitchCalc.Views
{
	public partial class HomeView : ContentPage, IViewFor<HomeViewViewModel>
	{
		public HomeView ()
		{
			InitializeComponent ();

			this.BindCommand(ViewModel, vm => vm.NavigateToProductFormPage, v => v.AddProductToolbarItem);
			this.Bind(ViewModel, vm => vm.SearchTerm, v => v.ProductSearchBar.Text);
			this.OneWayBind(ViewModel, vm => vm.CollectionView, v => v.ProductListView.ItemsSource);
			this.BindCommand(ViewModel, vm => vm.NavigateToProductPage, v => v.ProductListView, nameof(ListView.ItemTapped));

			Observable
				.FromEventPattern(ProductListView, nameof(ListView.ItemSelected))
				.Subscribe(_ => ProductListView.SelectedItem = null);
		}

		public HomeViewViewModel ViewModel
		{
			get { return (HomeViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(HomeViewViewModel), typeof(HomeView), new HomeViewViewModel());

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (HomeViewViewModel)value; }
		}
	}
}
