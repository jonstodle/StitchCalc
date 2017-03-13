using ReactiveUI;
using System;
using System.Reactive.Linq;
using Xamarin.Forms;
using System.Reactive.Disposables;
using System.Reactive;
using ReactiveUI.XamForms;
using StitchCalc.ViewModels;

namespace StitchCalc.Views
{
	public partial class ProductsView : ReactiveContentPage<ProductsViewModel>
	{
		public ProductsView(ProductsViewModel viewModel)
		{
			InitializeComponent();

			ViewModel = viewModel;

			this.Bind(ViewModel, vm => vm.SearchTerm, v => v.ProductSearchBar.Text);
			this.OneWayBind(ViewModel, vm => vm.ProductsView, v => v.ProductListView.ItemsSource, x => x.ToReactiveObservableList());
			this.Bind(ViewModel, vm => vm.SelectedProduct, v => v.ProductListView.SelectedItem);

			this.BindCommand(ViewModel, vm => vm.NavigateToProductFormPage, v => v.AddProductToolbarItem);
			this.BindCommand(ViewModel, vm => vm.NavigateToProductPage, v => v.ProductListView, nameof(ListView.ItemTapped));

			ProductListView.SelectedItem = null;
		}
	}
}
