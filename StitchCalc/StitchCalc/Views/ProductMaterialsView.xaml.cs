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
	public partial class ProductMaterialsView : ContentPage, IViewFor<ProductMaterialsViewViewModel>
	{
		public ProductMaterialsView ()
		{
			InitializeComponent ();
		}

		public ProductMaterialsViewViewModel ViewModel
		{
			get { return (ProductMaterialsViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(ProductMaterialsViewViewModel), typeof(ProductMaterialsView), new ProductMaterialsViewViewModel());

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ProductMaterialsViewViewModel)value; }
		}
	}
}
