using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Disposables;

using Xamarin.Forms;
using ReactiveUI.XamForms;
using StitchCalc.ViewModels;

namespace StitchCalc.Views
{
	public partial class ProductMaterialFormView : ReactiveContentPage<ProductMaterialFormViewViewModel>
	{
		public ProductMaterialFormView ()
		{
			InitializeComponent ();

			ViewModel = new ProductMaterialFormViewViewModel();

			this.WhenActivated(disposables => {
				this.OneWayBind(ViewModel, vm => vm.PageTitle, v => v.Title).DisposeWith(disposables);
				this.Bind(ViewModel, vm => vm.SelectedMaterialIndex, v => v.MaterialPicker.SelectedIndex).DisposeWith(disposables);
				this.Bind(ViewModel, vm => vm.Amount, v => v.AmountEntry.Text).DisposeWith(disposables);

				this.BindCommand(ViewModel, vm => vm.Save, v => v.SaveToolbarItem).DisposeWith(disposables);
				this.BindCommand(ViewModel, vm => vm.NavigateToMaterialFormView, v => v.AddMaterialButton).DisposeWith(disposables);
				this.BindCommand(ViewModel, vm => vm.Save, v => v.AmountEntry, nameof(Entry.Completed)).DisposeWith(disposables);

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
					})
					.DisposeWith(disposables);
			});
		}
	}
}
