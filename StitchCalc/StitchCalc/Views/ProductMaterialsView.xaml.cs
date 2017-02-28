using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Reactive.Disposables;

using Xamarin.Forms;
using ReactiveUI.XamForms;
using StitchCalc.ViewModels;

namespace StitchCalc.Views
{
    public partial class ProductMaterialsView : ReactiveContentPage<ProductMaterialsViewModel>
    {
        public ProductMaterialsView(ProductMaterialsViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.ProductMaterialsView, v => v.MaterialsListView.ItemsSource, x => new ReactiveObservableList<ProductMaterialViewModel>(x)).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.MaterialsPrice, v => v.SumLabel.Text, x => x.ToString("N2")).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedProductMaterial, v => v.MaterialsListView.SelectedItem).DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.AddProductMaterial, v => v.AddProductMaterialsToolbarItem).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.SelectedProductMaterial.Edit, v => v.MaterialsListView, nameof(ListView.ItemTapped)).DisposeWith(disposables);

                MaterialsListView.SelectedItem = null;
            });
        }
    }
}
