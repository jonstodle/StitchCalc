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

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.SearchTerm, v => v.ProductSearchBar.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.ProductsView, v => v.ProductListView.ItemsSource).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedProduct, v => v.ProductListView.SelectedItem).DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.NavigateToProductFormPage, v => v.AddProductToolbarItem).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.NavigateToProductPage, v => v.ProductListView, nameof(ListView.ItemTapped)).DisposeWith(disposables);

                ProductListView.SelectedItem = null;
            });
        }
    }
}
