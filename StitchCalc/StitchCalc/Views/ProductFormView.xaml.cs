using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System.Reactive.Linq;
using System.Reactive.Disposables;

using Xamarin.Forms;
using ReactiveUI.XamForms;

namespace StitchCalc.Views
{
	public partial class ProductFormView : ReactiveContentPage<ProductFormViewViewModel>
	{
		public ProductFormView()
		{
			InitializeComponent();

			ViewModel = new ProductFormViewViewModel();

			this.WhenActivated(disposables => {
				this.OneWayBind(ViewModel, vm => vm.PageTitle, v => v.Title).DisposeWith(disposables);
				this.Bind(ViewModel, vm => vm.Name, v => v.NameEntry.Text).DisposeWith(disposables);

				this.BindCommand(ViewModel, vm => vm.Save, v => v.SaveProductToolbarItem).DisposeWith(disposables);
				Observable
					.FromEventPattern(NameEntry, nameof(Entry.Completed))
					.ToSignal()
					.InvokeCommand(ViewModel, x => x.Save)
					.DisposeWith(disposables);
			});
		}
	}
}
