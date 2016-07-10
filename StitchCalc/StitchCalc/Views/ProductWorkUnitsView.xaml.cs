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
	public partial class ProductWorkUnitsView : ContentPage, IViewFor<ProductWorkUnitsViewViewModel>
	{
		public ProductWorkUnitsView ()
		{
			InitializeComponent ();
		}

		public ProductWorkUnitsViewViewModel ViewModel
		{
			get { return (ProductWorkUnitsViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(ProductWorkUnitsViewViewModel), typeof(ProductWorkUnitsView), new ProductWorkUnitsViewViewModel());

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ProductWorkUnitsViewViewModel)value; }
		}
	}
}
