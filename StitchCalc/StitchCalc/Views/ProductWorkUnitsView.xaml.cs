using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Reactive.Disposables;

using Xamarin.Forms;
using ReactiveUI.XamForms;
using StitchCalc.ViewModels;

namespace StitchCalc.Views
{
	public partial class ProductWorkUnitsView : ReactiveContentPage<ProductViewModel>
	{
		public ProductWorkUnitsView()
		{
			InitializeComponent();

			ViewModel = new ProductWorkUnitsViewViewModel();

			this.WhenActivated(disposables => {
				this.OneWayBind(ViewModel, vm => vm.Product.WorkUnits, v => v.WorkUnitsListView.ItemsSource).DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.WorkPrice, v => v.SumLabel.Text, x => x.ToString("N2")).DisposeWith(disposables);
				this.Bind(ViewModel, vm => vm.SelectedWorkUnit, v => v.WorkUnitsListView.SelectedItem).DisposeWith(disposables);

				this.BindCommand(ViewModel, vm => vm.NavigateToWorkUnitFormView, v => v.AddWorkUnitToolbarItem).DisposeWith(disposables);
				this.BindCommand(ViewModel, vm => vm.Edit, v => v.WorkUnitsListView, nameof(ListView.ItemTapped)).DisposeWith(disposables);

				WorkUnitsListView.SelectedItem = null;
			});
		}
	}
}
