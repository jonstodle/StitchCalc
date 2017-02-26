using ReactiveUI;
using System.Reactive.Linq;
using System.Reactive.Disposables;

using Xamarin.Forms;
using ReactiveUI.XamForms;
using StitchCalc.ViewModels;

namespace StitchCalc.Views
{
    public partial class WorkUnitFormView : ReactiveContentPage<WorkUnitFormViewModel>
    {
        public WorkUnitFormView(WorkUnitFormViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.PageTitle, v => v.Title).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.Name, v => v.NameEntry.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.Minutes, v => v.MinutesEntry.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.Charge, v => v.ChargeEntry.Text).DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.Save, v => v.SaveToolbarItem).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.Save, v => v.NameEntry, nameof(Entry.Completed)).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.Save, v => v.MinutesEntry, nameof(Entry.Completed)).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.Save, v => v.ChargeEntry, nameof(Entry.Completed)).DisposeWith(disposables);
            });
        }
    }
}
