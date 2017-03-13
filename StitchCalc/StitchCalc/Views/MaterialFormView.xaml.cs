using ReactiveUI;
using System.Reactive.Linq;
using System;

using Xamarin.Forms;
using System.Reactive;
using System.Reactive.Disposables;
using ReactiveUI.XamForms;
using StitchCalc.ViewModels;

namespace StitchCalc.Views
{
	public partial class MaterialFormView : ReactiveContentPage<MaterialFormViewModel>
	{
		public MaterialFormView(MaterialFormViewModel viewModel)
		{
			InitializeComponent();

			ViewModel = viewModel;

			this.OneWayBind(ViewModel, vm => vm.PageTitle, v => v.Title);
			this.Bind(ViewModel, vm => vm.Name, v => v.NameEntry.Text);
			this.Bind(ViewModel, vm => vm.Width, v => v.WidthEntry.Text);
			this.Bind(ViewModel, vm => vm.Price, v => v.PriceEntry.Text);
			this.Bind(ViewModel, vm => vm.Notes, v => v.NotesEditor.Text);

			this.BindCommand(ViewModel, vm => vm.Save, v => v.SaveToolbarItem);
			this.BindCommand(ViewModel, vm => vm.Save, v => v.NameEntry, nameof(Entry.Completed));
			this.BindCommand(ViewModel, vm => vm.Save, v => v.WidthEntry, nameof(Entry.Completed));
			this.BindCommand(ViewModel, vm => vm.Save, v => v.PriceEntry, nameof(Entry.Completed));
		}
	}
}
