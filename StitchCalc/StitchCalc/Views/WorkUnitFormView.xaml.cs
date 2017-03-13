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

                this.OneWayBind(ViewModel, vm => vm.PageTitle, v => v.Title);
                this.Bind(ViewModel, vm => vm.Name, v => v.NameEntry.Text);
                this.Bind(ViewModel, vm => vm.Minutes, v => v.MinutesEntry.Text);
                this.Bind(ViewModel, vm => vm.Charge, v => v.ChargeEntry.Text);

                this.BindCommand(ViewModel, vm => vm.Save, v => v.SaveToolbarItem);
                this.BindCommand(ViewModel, vm => vm.Save, v => v.NameEntry, nameof(Entry.Completed));
                this.BindCommand(ViewModel, vm => vm.Save, v => v.MinutesEntry, nameof(Entry.Completed));
                this.BindCommand(ViewModel, vm => vm.Save, v => v.ChargeEntry, nameof(Entry.Completed));
        }
    }
}
