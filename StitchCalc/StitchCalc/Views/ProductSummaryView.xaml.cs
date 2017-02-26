using Acr.UserDialogs;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;

using Xamarin.Forms;
using System.Reactive.Disposables;
using ReactiveUI.XamForms;
using System.Threading.Tasks;
using StitchCalc.ViewModels;

namespace StitchCalc.Views
{
	public partial class ProductSummaryView : ReactiveContentPage<ProductViewModel>
	{
		public ProductSummaryView(ProductViewModel viewModel)
		{
			InitializeComponent();

			ViewModel = new ProductSummaryViewViewModel();

			this.BindCommand(ViewModel, vm => vm.Edit, v => v.EditProductToolbarItem);

			this.WhenActivated(disposables =>
			{
				this.OneWayBind(ViewModel, vm => vm.Product.ChargeForMaterials, v => v.MaterialsStackLayout.Opacity, x => x ? 1 : .2).DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.MaterialsPrice, v => v.MaterialsCostLabel.Text, x => x.ToString("N2")).DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.IsMaterialsPriceMultiplied, v => v.MaterialsMultiplierLabel.IsVisible).DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.Product.MaterialsMultiplier, v => v.MaterialsMultiplierLabel.Text, x => $"x{x}").DisposeWith(disposables);

				this.OneWayBind(ViewModel, vm => vm.Product.ChargeForWork, v => v.WorkStackLayout.Opacity, x => x ? 1 : .2).DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.WorkPrice, v => v.WorkCostLabel.Text, x => x.ToString("N2")).DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.IsWorkPriceMultiplied, v => v.WorkMultiplierLabel.IsVisible).DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.Product.WorkMultiplier, v => v.WorkMultiplierLabel.Text, x => $"x{x}").DisposeWith(disposables);

				this.OneWayBind(ViewModel, vm => vm.TotalPrice, v => v.SumCostLabel.Text, x => x.ToString("N2")).DisposeWith(disposables);

				var materialsTaps = Observable.FromEventPattern(MaterialsStackLayoutTapGestureRecognizer, nameof(TapGestureRecognizer.Tapped))
											  .SelectMany(_ => Observable.FromAsync(() => DisplayActionSheet("Materials", _cancelString, _toggleString, _multiplyString)))
				                              .Publish()
				                              .RefCount();

				materialsTaps
					.Where(x => x == _toggleString)
					.ToSignal()
					.ObserveOn(RxApp.MainThreadScheduler)
					.InvokeCommand(ViewModel.ToggleChargeForMaterials)
					.DisposeWith(disposables);

				materialsTaps
					.Where(x => x == _multiplyString)
					.SelectMany(_ => Observable.FromAsync(() => CreateMultiplyPrompt("Materials")))
					.Where(x => x.Ok && x.Value.IsValidDouble())
					.Select(x => double.Parse(x.Value))
					.ObserveOn(RxApp.MainThreadScheduler)
					.InvokeCommand(ViewModel.SetMaterialsMultiplier)
					.DisposeWith(disposables);

				var workTaps = Observable.FromEventPattern(WorkStackLayoutTapGestureRecognizer, nameof(TapGestureRecognizer.Tapped))
										 .SelectMany(_ => Observable.FromAsync(() => DisplayActionSheet("Work", _cancelString, _toggleString, _multiplyString)))
										 .Publish()
										 .RefCount();

				workTaps
					.Where(x => x == _toggleString)
					.ToSignal()
					.ObserveOn(RxApp.MainThreadScheduler)
					.InvokeCommand(ViewModel.ToggleChargeForWork)
					.DisposeWith(disposables);

				workTaps
					.Where(x => x == _multiplyString)
					.SelectMany(_ => Observable.FromAsync(() => CreateMultiplyPrompt("Work")))
					.Where(x => x.Ok && x.Value.IsValidDouble())
					.Select(x => double.Parse(x.Value))
					.ObserveOn(RxApp.MainThreadScheduler)
					.InvokeCommand(ViewModel.SetWorkMultiplier)
					.DisposeWith(disposables);
			});
		}

		enum ChargeReasons { Materials, Work }
		string _cancelString = "Cancel";
		string _toggleString = "Toggle";
		string _multiplyString = "Multiply";
		Task<PromptResult> CreateMultiplyPrompt(string subjectToMultiply) => UserDialogs.Instance.PromptAsync($"Multiply {subjectToMultiply}", "Multiply", "OK", "Cancel", "0 to remove multiplier", inputType: InputType.Number);
	}
}
