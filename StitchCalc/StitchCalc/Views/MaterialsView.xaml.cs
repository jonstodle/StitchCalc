using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System;
using System.Reactive.Linq;

using Xamarin.Forms;

namespace StitchCalc.Views
{
	public partial class MaterialsView : ContentPage, IViewFor<MaterialsViewViewModel>
	{
		public MaterialsView ()
		{
			InitializeComponent ();

			ViewModel = new MaterialsViewViewModel();

			this.Bind(ViewModel, vm => vm.SearchTerm, v => v.MaterialsSearchBar.Text);
			this.OneWayBind(ViewModel, vm => vm.CollectionView, v => v.MaterialsListView.ItemsSource);
			this.Bind(ViewModel, vm => vm.SelectedMaterial, v => v.MaterialsListView.SelectedItem);

				this.BindCommand(ViewModel, vm => vm.NavigateToMaterialFormView, v => v.AddMaterialToolbarItem);
				this.BindCommand(ViewModel, vm => vm.Edit, v => v.MaterialsListView, nameof(ListView.ItemTapped));
		}

		public MaterialsViewViewModel ViewModel
		{
			get { return (MaterialsViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(MaterialsViewViewModel), typeof(MaterialsView), null);

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (MaterialsViewViewModel)value; }
		}
	}
}
