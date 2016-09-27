using Acr.UserDialogs;
using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System;
using System.Reactive;
using System.Reactive.Linq;

using Xamarin.Forms;

namespace StitchCalc.Views
{
	public partial class ProductSummaryView : ContentPage, IViewFor<ProductSummaryViewViewModel>
	{
		public ProductSummaryView ()
		{
			InitializeComponent ();

			ViewModel = new ProductSummaryViewViewModel();

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

			this.WhenActivated(d =>
			{
				d(Observable
					.FromEventPattern(MaterialsStackLayoutTapGestureRecognizer, nameof(TapGestureRecognizer.Tapped))
					.Subscribe(_ => ShowActionSheet("Materials", ChargeReasons.Materials)));
				d(Observable
					.FromEventPattern(WorkStackLayoutTapGestureRecognizer, nameof(TapGestureRecognizer.Tapped))
					.Subscribe(_ => ShowActionSheet("Work", ChargeReasons.Work)));
			});

			

			
		}

		enum ChargeReasons { Materials, Work }
		async void ShowActionSheet(string title, ChargeReasons reason)
		{
			var actions = new string[] {reason.ToString(), "Cancel", "Toggle", "Multiply" };

			var result = await DisplayActionSheet(actions[0], actions[1], null, actions[2], actions[3]);

			if (result == actions[2])
			{
				if (reason == ChargeReasons.Materials) { Observable.Return(Unit.Default).InvokeCommand(ViewModel.Model.ToggleChargeForMaterials); }
				else if(reason == ChargeReasons.Work) { Observable.Return(Unit.Default).InvokeCommand(ViewModel.Model.ToggleChargeForWork); }
			}
			else if (result == actions[3])
			{
				var multiplyResult = await UserDialogs.Instance.PromptAsync($"Multiply {reason.ToString()}", "Multiply", "OK", "Cancel", "0 to remove multiplier", inputType: InputType.Number);

				if (multiplyResult.Ok)
				{
					if (reason == ChargeReasons.Materials) { Observable.Return(multiplyResult.Value).InvokeCommand<string>(ViewModel.Model.SetMaterialsMultiplier); }
					else if (reason == ChargeReasons.Work) { Observable.Return(multiplyResult.Value).InvokeCommand<string>(ViewModel.Model.SetWorkMultiplier); }
				}
			}
		}



		public ProductSummaryViewViewModel ViewModel
		{
			get { return (ProductSummaryViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(ProductSummaryViewViewModel), typeof(ProductSummaryView), null);

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ProductSummaryViewViewModel)value; }
		}
	}
}
