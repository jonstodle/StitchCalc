using ReactiveUI;
using ReactiveUI.XamForms;
using StitchCalc.ViewModels.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

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
			this.Bind(ViewModel, vm => vm.SelectedProduct, v => v.ProductListView.SelectedItem);

			Observable.FromEventPattern(ProductListView, nameof(ListView.ItemTapped))
				.InvokeCommand(ViewModel, x => x.NavigateToProductPage);
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
