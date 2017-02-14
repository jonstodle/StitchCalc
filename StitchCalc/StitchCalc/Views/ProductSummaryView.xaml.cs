using Acr.UserDialogs;
using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System;
using System.Reactive;
using System.Reactive.Linq;

using Xamarin.Forms;
using System.Reactive.Disposables;
using ReactiveUI.XamForms;

namespace StitchCalc.Views
{
	public partial class ProductSummaryView : ReactiveContentPage<ProductSummaryViewViewModel>
	{
		public ProductSummaryView ()
		{
			InitializeComponent ();

			ViewModel = new ProductSummaryViewViewModel();

			this.BindCommand(ViewModel, vm => vm.Edit, v => v.EditProductToolbarItem);

			this.WhenActivated(disposables => {
				this.OneWayBind(ViewModel, vm => vm.Model.ChargeForMaterials, v => v.MaterialsStackLayout.Opacity, x=> x ? 1 : .2).DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.Model.MaterialsPrice, v => v.MaterialsCostLabel.Text, x => x.ToString("N2")).DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.Model.IsMaterialsPriceMultiplied, v => v.MaterialsMultiplierLabel.IsVisible).DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.Model.MaterialsMultiplier, v => v.MaterialsMultiplierLabel.Text, x => $"x{x}").DisposeWith(disposables);

				this.OneWayBind(ViewModel, vm => vm.Model.ChargeForWork, v => v.WorkStackLayout.Opacity, x => x ? 1: .2).DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.Model.WorkPrice, v => v.WorkCostLabel.Text, x => x.ToString("N2")).DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.Model.IsWorkPriceMultiplied, v => v.WorkMultiplierLabel.IsVisible).DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.Model.WorkMultiplier, v => v.WorkMultiplierLabel.Text, x=> $"x{x}").DisposeWith(disposables);

				this.OneWayBind(ViewModel, vm => vm.Model.TotalPrice, v => v.SumCostLabel.Text, x => x.ToString("N2")).DisposeWith(disposables);

				Observable
					.FromEventPattern(MaterialsStackLayoutTapGestureRecognizer, nameof(TapGestureRecognizer.Tapped))
					.Subscribe(_ => ShowActionSheet("Materials", ChargeReasons.Materials))
					.DisposeWith(disposables);
				Observable
					.FromEventPattern(WorkStackLayoutTapGestureRecognizer, nameof(TapGestureRecognizer.Tapped))
					.Subscribe(_ => ShowActionSheet("Work", ChargeReasons.Work))
					.DisposeWith(disposables);
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
	}
}
