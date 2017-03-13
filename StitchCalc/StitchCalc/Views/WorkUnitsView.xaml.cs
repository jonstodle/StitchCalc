using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Reactive.Disposables;

using Xamarin.Forms;
using ReactiveUI.XamForms;
using StitchCalc.ViewModels;

namespace StitchCalc.Views
{
	public partial class WorkUnitsView : ReactiveContentPage<WorkUnitsViewModel>
	{
		public WorkUnitsView(WorkUnitsViewModel viewModel)
		{
			InitializeComponent();

			ViewModel = viewModel;

			this.OneWayBind(ViewModel, vm => vm.WorkUnitsView, v => v.WorkUnitsListView.ItemsSource, x => x.ToReactiveObservableList());
			this.OneWayBind(ViewModel, vm => vm.WorkPrice, v => v.SumLabel.Text, x => x.ToString("N2"));
			this.Bind(ViewModel, vm => vm.SelectedWorkUnit, v => v.WorkUnitsListView.SelectedItem);

			this.BindCommand(ViewModel, vm => vm.AddWorkUnit, v => v.AddWorkUnitToolbarItem);
			this.BindCommand(ViewModel, vm => vm.SelectedWorkUnit.Edit, v => v.WorkUnitsListView, nameof(ListView.ItemTapped));

			WorkUnitsListView.SelectedItem = null;
		}
	}
}
