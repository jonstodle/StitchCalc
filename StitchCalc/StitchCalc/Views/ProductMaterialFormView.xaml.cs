using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace StitchCalc.Views
{
	public partial class ProductMaterialFormView : ContentPage, IViewFor<ProductMaterialFormViewViewModel>
	{
		public ProductMaterialFormView ()
		{
			InitializeComponent ();

			this.OneWayBind(ViewModel, vm => vm.PageTitle, v => v.Title);
		}

		public ProductMaterialFormViewViewModel ViewModel
		{
			get { return (ProductMaterialFormViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(ProductMaterialFormViewViewModel), typeof(ProductMaterialFormView), new ProductMaterialFormViewViewModel());

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ProductMaterialFormViewViewModel)value; }
		}
	}
}
