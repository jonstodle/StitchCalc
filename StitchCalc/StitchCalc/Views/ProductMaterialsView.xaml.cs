﻿using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System;
using System.Reactive.Linq;

using Xamarin.Forms;

namespace StitchCalc.Views
{
	public partial class ProductMaterialsView : ContentPage, IViewFor<ProductMaterialsViewViewModel>
	{
		public ProductMaterialsView()
		{
			InitializeComponent();

			ViewModel = new ProductMaterialsViewViewModel();

			this.OneWayBind(ViewModel, vm => vm.Product.Materials, v => v.MaterialsListView.ItemsSource);
			this.OneWayBind(ViewModel, vm => vm.Product.MaterialsPrice, v => v.SumLabel.Text, x => x.ToString("N2"));
			this.Bind(ViewModel, vm => vm.SelectedProductMaterial, v => v.MaterialsListView.SelectedItem);

			this.BindCommand(ViewModel, vm => vm.NavigateToProductMaterialFormView, v => v.AddProductMaterialsToolbarItem);
			this.BindCommand(ViewModel, vm => vm.Edit, v => v.MaterialsListView, nameof(ListView.ItemTapped));
		}

		public ProductMaterialsViewViewModel ViewModel
		{
			get { return (ProductMaterialsViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(ProductMaterialsViewViewModel), typeof(ProductMaterialsView), null);

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ProductMaterialsViewViewModel)value; }
		}
	}
}
