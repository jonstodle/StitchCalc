using ReactiveUI;
using System.Linq;
using System.Reactive.Linq;

using Xamarin.Forms;
using System.Reactive;
using StitchCalc.ViewModels;
using System;
using System.Reactive.Disposables;

namespace StitchCalc.Views
{
	public class ProductView : TabbedPage, IViewFor<ProductViewModel>
	{
		public ProductView(ProductViewModel viewModel)
		{
			ViewModel = viewModel;

			Children.Add(new ProductSummaryView(ViewModel));
			Children.Add(new ProductMaterialsView(new ProductMaterialsViewModel(ViewModel.Product)));
			Children.Add(new WorkUnitsView(new WorkUnitsViewModel(ViewModel.Product)));

			this.OneWayBind(ViewModel, vm => vm.Product.Name, v => v.Title);
		}

		public ProductViewModel ViewModel
		{
			get { return (ProductViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(ProductViewModel), typeof(ProductView), null);

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ProductViewModel)value; }
		}

		public IObservable<Unit> Activated => Observable.FromEventPattern(this, nameof(this.Appearing)).ToSignal();

		public IObservable<Unit> Deactivated => Observable.FromEventPattern(this, nameof(this.Disappearing)).ToSignal();
	}
}
