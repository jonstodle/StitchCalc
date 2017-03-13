using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Reactive.Disposables;

using Xamarin.Forms;
using ReactiveUI.XamForms;
using StitchCalc.ViewModels;

namespace StitchCalc.Views
{
	public partial class MaterialsView : ReactiveContentPage<MaterialsViewModel>
	{
		public MaterialsView(MaterialsViewModel viewModel)
		{
			InitializeComponent();

			ViewModel = viewModel;

			this.Bind(ViewModel, vm => vm.SearchTerm, v => v.MaterialsSearchBar.Text);
			this.OneWayBind(ViewModel, vm => vm.MaterialsView, v => v.MaterialsListView.ItemsSource, x => x.ToReactiveObservableList());
			this.Bind(ViewModel, vm => vm.SelectedMaterial, v => v.MaterialsListView.SelectedItem);

			this.BindCommand(ViewModel, vm => vm.AddMaterial, v => v.AddMaterialToolbarItem);
			this.BindCommand(ViewModel, vm => vm.SelectedMaterial.Edit, v => v.MaterialsListView, nameof(ListView.ItemTapped));

			MaterialsListView.SelectedItem = null;
		}
	}
}
