using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Reactive.Disposables;

using Xamarin.Forms;
using ReactiveUI.XamForms;
using StitchCalc.ViewModels;

namespace StitchCalc.Views
{
    public partial class ProductMaterialsView : ReactiveContentPage<ProductViewModel>
    {
        public ProductMaterialsView(ProductViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Product.Materials, v => v.MaterialsListView.ItemsSource).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.MaterialsPrice, v => v.SumLabel.Text, x => x.ToString("N2")).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedProductMaterial, v => v.MaterialsListView.SelectedItem).DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.NavigateToProductMaterialFormView, v => v.AddProductMaterialsToolbarItem).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.Edit, v => v.MaterialsListView, nameof(ListView.ItemTapped)).DisposeWith(disposables);

                MaterialsListView.SelectedItem = null;
            });
        }
    }
}
