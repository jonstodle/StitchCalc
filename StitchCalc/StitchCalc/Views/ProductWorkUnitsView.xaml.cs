using ReactiveUI;
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
	public partial class ProductWorkUnitsView : ContentPage, IViewFor<ProductWorkUnitsViewViewModel>
	{
		public ProductWorkUnitsView ()
		{
			InitializeComponent ();

			this.BindCommand(ViewModel, vm => vm.NavigateToWorkUnitFormView, v => v.AddWorkUnitToolbarItem);
			this.OneWayBind(ViewModel, vm => vm.Model.WorkUnits, v => v.WorkUnitsListView.ItemsSource);
			this.OneWayBind(ViewModel, vm => vm.Model.WorkPrice, v => v.SumLabel.Text, x => x.ToString("N2"));
			this.Bind(ViewModel, vm => vm.SelectedWorkUnit, v => v.WorkUnitsListView.SelectedItem);

			Observable.FromEventPattern(WorkUnitsListView, nameof(ListView.ItemTapped))
				.InvokeCommand(ViewModel, x => x.Edit);
		}

		public ProductWorkUnitsViewViewModel ViewModel
		{
			get { return (ProductWorkUnitsViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(ProductWorkUnitsViewViewModel), typeof(ProductWorkUnitsView), new ProductWorkUnitsViewViewModel());

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ProductWorkUnitsViewViewModel)value; }
		}
	}
}
