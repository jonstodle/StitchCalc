using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace StitchCalc.Views
{
	public partial class MaterialsView : ContentPage, IViewFor<MaterialsViewViewModel>
	{
		public MaterialsView ()
		{
			InitializeComponent ();

			this.BindCommand(ViewModel, vm => vm.NavigateToMaterialFormView, v => v.AddMaterialToolbarItem);
			this.Bind(ViewModel, vm => vm.SearchTerm, v => v.MaterialsSearchBar.Text);
			this.OneWayBind(ViewModel, vm => vm.Materials, v => v.MaterialsListView.ItemsSource);
		}

		public MaterialsViewViewModel ViewModel
		{
			get { return (MaterialsViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(MaterialsViewViewModel), typeof(MaterialsView), new MaterialsViewViewModel());

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (MaterialsViewViewModel)value; }
		}
	}
}
