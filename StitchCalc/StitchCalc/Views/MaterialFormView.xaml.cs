using ReactiveUI;
using StitchCalc.ViewModels.Models;
using StitchCalc.ViewModels.Views;
using System.Reactive.Linq;
using System;

using Xamarin.Forms;
using System.Reactive;
using System.Reactive.Disposables;
using ReactiveUI.XamForms;

namespace StitchCalc.Views
{
	public partial class MaterialFormView : ReactiveContentPage<MaterialFormViewViewModel>
	{
		public MaterialFormView()
		{
			InitializeComponent();

			ViewModel = new MaterialFormViewViewModel();

			this.WhenActivated(disposables => {
				this.OneWayBind(ViewModel, vm => vm.PageTitle, v => v.Title).DisposeWith(disposables);
				this.Bind(ViewModel, vm => vm.Name, v => v.NameEntry.Text).DisposeWith(disposables);
				this.Bind(ViewModel, vm => vm.Width, v => v.WidthEntry.Text).DisposeWith(disposables);
				this.Bind(ViewModel, vm => vm.Price, v => v.PriceEntry.Text).DisposeWith(disposables);
				this.Bind(ViewModel, vm => vm.Notes, v => v.NotesEditor.Text).DisposeWith(disposables);

				this.BindCommand(ViewModel, vm => vm.Save, v => v.SaveToolbarItem).DisposeWith(disposables);
				Observable
					.Merge(
					Observable.FromEventPattern(NameEntry, nameof(Entry.Completed)),
					Observable.FromEventPattern(WidthEntry, nameof(Entry.Completed)),
					Observable.FromEventPattern(PriceEntry, nameof(Entry.Completed)))
					.ToSignal()
					.InvokeCommand(ViewModel, x => x.Save)
					.DisposeWith(disposables);
			});
		}
	}
}
