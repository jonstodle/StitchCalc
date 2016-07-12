﻿using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace StitchCalc.Views
{
	public partial class ProductFormView : ContentPage, IViewFor<ProductFormViewViewModel>
	{
		public ProductFormView ()
		{
			InitializeComponent ();

			this.OneWayBind(ViewModel, vm => vm.PageTitle, v => v.Title);
			this.Bind(ViewModel, vm => vm.Name, v => v.NameEntry.Text);
			this.BindCommand(ViewModel, vm => vm.Save, v => v.SaveProductToolbarItem);
		}

		public ProductFormViewViewModel ViewModel
		{
			get { return (ProductFormViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(ProductFormViewViewModel), typeof(ProductFormView), new ProductFormViewViewModel());

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ProductFormViewViewModel)value; }
		}
	}
}
