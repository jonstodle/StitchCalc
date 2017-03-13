using ReactiveUI;
using System.Reactive.Linq;
using System.Reactive.Disposables;

using Xamarin.Forms;
using ReactiveUI.XamForms;
using StitchCalc.ViewModels;

namespace StitchCalc.Views
{
    public partial class ProductFormView : ReactiveContentPage<ProductFormViewModel>
    {
        public ProductFormView(ProductFormViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;

                this.OneWayBind(ViewModel, vm => vm.PageTitle, v => v.Title);
                this.Bind(ViewModel, vm => vm.Name, v => v.NameEntry.Text);

                this.BindCommand(ViewModel, vm => vm.Save, v => v.SaveProductToolbarItem);
                this.BindCommand(ViewModel, vm => vm.Save, v => v.NameEntry, nameof(Entry.Completed));
        }
    }
}
