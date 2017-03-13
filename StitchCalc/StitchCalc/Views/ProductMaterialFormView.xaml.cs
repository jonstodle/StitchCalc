using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Disposables;

using Xamarin.Forms;
using ReactiveUI.XamForms;
using StitchCalc.ViewModels;
using System.Collections.Specialized;

namespace StitchCalc.Views
{
    public partial class ProductMaterialFormView : ReactiveContentPage<ProductMaterialFormViewModel>
    {
        public ProductMaterialFormView(ProductMaterialFormViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;

                MaterialPicker.Items.Clear();
				foreach (var material in ViewModel.Materials) MaterialPicker.Items.Add(material.Name);

				this.OneWayBind(ViewModel, vm => vm.PageTitle, v => v.Title);
                this.Bind(ViewModel, vm => vm.SelectedMaterialIndex, v => v.MaterialPicker.SelectedIndex);
                this.Bind(ViewModel, vm => vm.Amount, v => v.AmountEntry.Text);

                this.BindCommand(ViewModel, vm => vm.Save, v => v.SaveToolbarItem);
                this.BindCommand(ViewModel, vm => vm.AddMaterial, v => v.AddMaterialButton);
                this.BindCommand(ViewModel, vm => vm.Save, v => v.AmountEntry, nameof(Entry.Completed));
        }
    }
}
