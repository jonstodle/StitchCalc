using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System;
using System.Linq;
using System.Reactive.Linq;

using Xamarin.Forms;

namespace StitchCalc.Views
{
	public partial class ProductMaterialFormView : ContentPage, IViewFor<ProductMaterialFormViewViewModel>
	{
		public ProductMaterialFormView ()
		{
			InitializeComponent ();

			ViewModel = new ProductMaterialFormViewViewModel();

			this.OneWayBind(ViewModel, vm => vm.PageTitle, v => v.Title);
			this.Bind(ViewModel, vm => vm.SelectedMaterialIndex, v => v.MaterialPicker.SelectedIndex);
			this.Bind(ViewModel, vm => vm.Amount, v => v.AmountEntry.Text);

				this.BindCommand(ViewModel, vm => vm.Save, v => v.SaveToolbarItem);
				this.BindCommand(ViewModel, vm => vm.NavigateToMaterialFormView, v => v.AddMaterialButton);
			Observable
					.FromEventPattern(AmountEntry, nameof(Entry.Completed))
					.InvokeCommand(ViewModel, x => x.Save);
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

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(ProductMaterialFormViewViewModel), typeof(ProductMaterialFormView), null);

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ProductMaterialFormViewViewModel)value; }
		}
	}
}
