using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System;
using System.Reactive.Linq;

using Xamarin.Forms;

namespace StitchCalc.Views
{
	public partial class ProductWorkUnitsView : ContentPage, IViewFor<ProductWorkUnitsViewViewModel>
	{
		public ProductWorkUnitsView()
		{
			InitializeComponent();

			ViewModel = new ProductWorkUnitsViewViewModel();

			this.OneWayBind(ViewModel, vm => vm.Model.WorkUnits, v => v.WorkUnitsListView.ItemsSource);
			this.OneWayBind(ViewModel, vm => vm.Model.WorkPrice, v => v.SumLabel.Text, x => x.ToString("N2"));

				this.BindCommand(ViewModel, vm => vm.NavigateToWorkUnitFormView, v => v.AddWorkUnitToolbarItem);
				this.BindCommand(ViewModel, vm => vm.Edit, v => v.WorkUnitsListView, nameof(ListView.ItemTapped));
				Observable.FromEventPattern(WorkUnitsListView, nameof(ListView.ItemSelected))
					.Subscribe(_ => WorkUnitsListView.SelectedItem = null);


		}

		public ProductWorkUnitsViewViewModel ViewModel
		{
			get { return (ProductWorkUnitsViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(ProductWorkUnitsViewViewModel), typeof(ProductWorkUnitsView), null);

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ProductWorkUnitsViewViewModel)value; }
		}
	}
}
