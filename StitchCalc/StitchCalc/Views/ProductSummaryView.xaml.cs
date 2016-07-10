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
	public partial class ProductSummaryView : ContentPage, IViewFor<ProductSummaryViewViewModel>
	{
		public ProductSummaryView ()
		{
			InitializeComponent ();
		}

		public ProductSummaryViewViewModel ViewModel
		{
			get { return (ProductSummaryViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(ProductSummaryViewViewModel), typeof(ProductSummaryView), new ProductSummaryViewViewModel());

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ProductSummaryViewViewModel)value; }
		}
	}
}
