using ReactiveUI;
using System.Reactive.Linq;
using System.Reactive.Disposables;

using Xamarin.Forms;
using ReactiveUI.XamForms;
using StitchCalc.ViewModels;

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
				this.BindCommand(ViewModel, vm => vm.Save, v => v.NameEntry, nameof(Entry.Completed)).DisposeWith(disposables);
			});
		}
	}
}
