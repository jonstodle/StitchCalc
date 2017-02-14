using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System.Reactive.Linq;
using System.Reactive.Disposables;

using Xamarin.Forms;
using ReactiveUI.XamForms;

namespace StitchCalc.Views
{
	public partial class WorkUnitFormView : ReactiveContentPage<WorkUnitFormViewViewModel>
	{
		public WorkUnitFormView()
		{
			InitializeComponent();

			ViewModel = new WorkUnitFormViewViewModel();

			this.WhenActivated(disposables => {
				this.OneWayBind(ViewModel, vm => vm.PageTitle, v => v.Title).DisposeWith(disposables);
				this.Bind(ViewModel, vm => vm.Name, v => v.NameEntry.Text).DisposeWith(disposables);
				this.Bind(ViewModel, vm => vm.Minutes, v => v.MinutesEntry.Text).DisposeWith(disposables);
				this.Bind(ViewModel, vm => vm.Charge, v => v.ChargeEntry.Text).DisposeWith(disposables);

				this.BindCommand(ViewModel, vm => vm.Save, v => v.SaveToolbarItem).DisposeWith(disposables);
				Observable
					.Merge(
					Observable.FromEventPattern(NameEntry, nameof(Entry.Completed)),
					Observable.FromEventPattern(MinutesEntry, nameof(Entry.Completed)),
					Observable.FromEventPattern(ChargeEntry, nameof(Entry.Completed)))
					.ToSignal()
					.InvokeCommand(ViewModel, x => x.Save)
					.DisposeWith(disposables);
			});
		}
	}
}
