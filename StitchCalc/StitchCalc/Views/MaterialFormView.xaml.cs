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
    public partial class MaterialFormView : ReactiveContentPage<MaterialFormViewViewModel>
    {
        public MaterialFormView()
        {
            InitializeComponent();

            ViewModel = new MaterialFormViewViewModel();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.PageTitle, v => v.Title).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.Name, v => v.NameEntry.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.Width, v => v.WidthEntry.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.Price, v => v.PriceEntry.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.Notes, v => v.NotesEditor.Text).DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.Save, v => v.SaveToolbarItem).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.Save, v => v.NameEntry, nameof(Entry.Completed)).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.Save, v => v.WidthEntry, nameof(Entry.Completed)).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.Save, v => v.PriceEntry, nameof(Entry.Completed)).DisposeWith(disposables);
            });
        }
    }
}
