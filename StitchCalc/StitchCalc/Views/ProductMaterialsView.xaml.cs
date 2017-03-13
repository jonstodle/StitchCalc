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

			this.OneWayBind(ViewModel, vm => vm.ProductMaterialsView, v => v.MaterialsListView.ItemsSource, x => x.ToReactiveObservableList());
			this.OneWayBind(ViewModel, vm => vm.MaterialsPrice, v => v.SumLabel.Text, x => x.ToString("N2"));
			this.Bind(ViewModel, vm => vm.SelectedProductMaterial, v => v.MaterialsListView.SelectedItem);

			this.BindCommand(ViewModel, vm => vm.AddProductMaterial, v => v.AddProductMaterialsToolbarItem);
			this.BindCommand(ViewModel, vm => vm.SelectedProductMaterial.Edit, v => v.MaterialsListView, nameof(ListView.ItemTapped));

			MaterialsListView.SelectedItem = null;
		}
	}
}
