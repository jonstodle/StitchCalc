using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System.Reactive.Linq;

using Xamarin.Forms;

namespace StitchCalc.Views
{
	public partial class WorkUnitFormView : ContentPage, IViewFor<WorkUnitFormViewViewModel>
	{
		public WorkUnitFormView()
		{
			InitializeComponent();

			ViewModel = new WorkUnitFormViewViewModel();

			this.OneWayBind(ViewModel, vm => vm.PageTitle, v => v.Title);
			this.Bind(ViewModel, vm => vm.Name, v => v.NameEntry.Text);
			this.Bind(ViewModel, vm => vm.Minutes, v => v.MinutesEntry.Text);
			this.Bind(ViewModel, vm => vm.Charge, v => v.ChargeEntry.Text);

				this.BindCommand(ViewModel, vm => vm.Save, v => v.SaveToolbarItem);
			Observable
				.Merge(
				Observable.FromEventPattern(NameEntry, nameof(Entry.Completed)),
				Observable.FromEventPattern(MinutesEntry, nameof(Entry.Completed)),
				Observable.FromEventPattern(ChargeEntry, nameof(Entry.Completed)))
				.InvokeCommand(ViewModel, x => x.Save);
		}

		public WorkUnitFormViewViewModel ViewModel
		{
			get { return (WorkUnitFormViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(WorkUnitFormViewViewModel), typeof(WorkUnitFormView), null);

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (WorkUnitFormViewViewModel)value; }
		}
	}
}
