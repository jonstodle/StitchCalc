using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System.Reactive.Linq;

using Xamarin.Forms;

namespace StitchCalc.Views
{
	public partial class ProductFormView : ContentPage, IViewFor<ProductFormViewViewModel>
	{
		public ProductFormView()
		{
			InitializeComponent();

			ViewModel = new ProductFormViewViewModel();

			this.OneWayBind(ViewModel, vm => vm.PageTitle, v => v.Title);
			this.Bind(ViewModel, vm => vm.Name, v => v.NameEntry.Text);

			this.WhenActivated(d =>
			{
				d(this.BindCommand(ViewModel, vm => vm.Save, v => v.SaveProductToolbarItem));
				d(Observable
					.FromEventPattern(NameEntry, nameof(Entry.Completed))
					.InvokeCommand(ViewModel, x => x.Save));
			});
		}

		public ProductFormViewViewModel ViewModel
		{
			get { return (ProductFormViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(ProductFormViewViewModel), typeof(ProductFormView), null);

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ProductFormViewViewModel)value; }
		}
	}
}
