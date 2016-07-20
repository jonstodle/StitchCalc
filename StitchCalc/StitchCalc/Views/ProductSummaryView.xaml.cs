﻿using ReactiveUI;
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
	public partial class ProductSummaryView : ContentPage, IViewFor<ProductSummaryViewViewModel>
	{
		public ProductSummaryView ()
		{
			InitializeComponent ();

			this.BindCommand(ViewModel, vm => vm.Edit, v => v.EditProductToolbarItem);

			this.OneWayBind(ViewModel, vm => vm.Model.ChargeForMaterials, v => v.MaterialsStackLayout.Opacity, x=> x ? 1 : .2);
			this.OneWayBind(ViewModel, vm => vm.Model.MaterialsPrice, v => v.MaterialsCostLabel.Text, x => x.ToString("N2"));
			this.OneWayBind(ViewModel, vm => vm.Model.IsMaterialsPriceMultiplied, v => v.MaterialsMultiplierLabel.IsVisible);
			this.OneWayBind(ViewModel, vm => vm.Model.MaterialsMultiplier, v => v.MaterialsMultiplierLabel.Text, x => $"x{x}");

			this.OneWayBind(ViewModel, vm => vm.Model.ChargeForWork, v => v.WorkStackLayout.Opacity, x => x ? 1: .5);
			this.OneWayBind(ViewModel, vm => vm.Model.WorkPrice, v => v.WorkCostLabel.Text, x => x.ToString("N2"));
			this.OneWayBind(ViewModel, vm => vm.Model.IsWorkPriceMultiplied, v => v.WorkMultiplierLabel.IsVisible);
			this.OneWayBind(ViewModel, vm => vm.Model.WorkMultiplier, v => v.WorkMultiplierLabel.Text, x=> $"x{x}");

			this.OneWayBind(ViewModel, vm => vm.Model.TotalPrice, v => v.SumCostLabel.Text, x => x.ToString("N2"));

			Observable
				.FromEventPattern(MaterialsStackLayoutTapGestureRecognizer, nameof(TapGestureRecognizer.Tapped))
				.Subscribe(_ => ShowActionSheet("Materials", ChargeReasons.Materials));

			Observable
				.FromEventPattern(WorkStackLayoutTapGestureRecognizer, nameof(TapGestureRecognizer.Tapped))
				.Subscribe(_ => ShowActionSheet("Work", ChargeReasons.Work));
		}

		enum ChargeReasons { Materials, Work }
		async void ShowActionSheet(string title, ChargeReasons reason)
		{
			var actions = new string[] { "Cancel", "Toggle", "Multiply" };

			var result = await DisplayActionSheet("", actions[0], null, actions[1], actions[2]);

			if (result == actions[1])
			{
				if (reason == ChargeReasons.Materials) { ViewModel.Model.ToggleChargeForMaterials.Execute(null); }
				else if(reason == ChargeReasons.Work) { ViewModel.Model.ToggleChargeForWork.Execute(null); }
			}
			else if (result == actions[2])
			{
				
			}
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
