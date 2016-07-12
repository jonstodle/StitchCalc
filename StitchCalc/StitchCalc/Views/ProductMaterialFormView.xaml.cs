using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
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

			this.BindCommand(ViewModel, vm => vm.Save, v => v.SaveToolbarItem);
			this.OneWayBind(ViewModel, vm => vm.PageTitle, v => v.Title);
			this.Bind(ViewModel, vm => vm.SelectedMaterialIndex, v => v.MaterialPicker.SelectedIndex);
			this.Bind(ViewModel, vm => vm.Amount, v => v.AmountEntry.Text);

			ViewModel
				.Materials
				.Changed
				.Select(_ => ViewModel.Materials)
				.StartWith(ViewModel.Materials)
				.Subscribe(items =>
				{
					MaterialPicker.Items.Clear();
					foreach (var item in items)
					{
						MaterialPicker.Items.Add(item.Name);
					}
				});
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
