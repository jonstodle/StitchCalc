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

			this.BindCommand(ViewModel, vm => vm.Edit, v => v.EditProductToolbarItem);
			this.OneWayBind(ViewModel, vm => vm.Model.MaterialsPrice, v => v.MaterialsCostLabel.Text, x => x.ToString("N2"));
			this.OneWayBind(ViewModel, vm => vm.Model.WorkPrice, v => v.WorkCostLabel.Text, x => x.ToString("N2"));
			this.OneWayBind(ViewModel, vm => vm.Model.TotalPrice, v => v.SumCostLabel.Text, x => x.ToString("N2"));
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
