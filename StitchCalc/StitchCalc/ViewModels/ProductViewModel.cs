using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Realms;
using StitchCalc.Models;
using StitchCalc.Services;
using StitchCalc.Views;

namespace StitchCalc.ViewModels
{
	public class ProductViewModel : ViewModelBase
	{
		public ProductViewModel(Product product)
		{
			_product = product;

            _add = ReactiveCommand.CreateFromObservable(() => Observable.FromAsync(() => NavigationService.NavigateTo(new ProductFormView(new ProductFormViewModel()))));

            _edit = ReactiveCommand.CreateFromObservable(() => Observable.FromAsync(() => NavigationService.NavigateTo(new ProductFormView(new ProductFormViewModel(_product)))));

            _toggleChargeForMaterials = ReactiveCommand.Create(() => DBService.Write(realm => _product.ChargeForMaterials = !_product.ChargeForMaterials));

			_setMaterialsMultiplier = ReactiveCommand.Create<double, Unit>(value => { DBService.Write(realm => _product.MaterialsMultiplier = value); return Unit.Default; });

			_toggleChargeForWork = ReactiveCommand.Create(() => DBService.Write(realm => _product.ChargeForWork = !_product.ChargeForWork));

			_setWorkMultiplier = ReactiveCommand.Create<double, Unit>(value => { DBService.Write(realm => _product.WorkMultiplier = value); return Unit.Default; });

			_materialsPrice = Materials.CollectionChanges()
									   .Select(_ => Materials.Sum(x => x.Price))
									   .ToProperty(this, x => x.MaterialsPrice);

			_isMaterialsPriceMultiplied = this.WhenAnyValue(x => x.Product.MaterialsMultiplier)
				.Select(x => x > 0)
				.ToProperty(this, x => x.IsMaterialsPriceMultiplied);

			_workUnitsPrice = WorkUnits.CollectionChanges()
									   .Select(_ => WorkUnits.Sum(x => x.Charge * x.Minutes))
									   .ToProperty(this, x => x.WorkUnitsPrice);

			_isWorkPriceMultiplied = this.WhenAnyValue(x => x.Product.WorkMultiplier)
				.Select(x => x > 0)
				.ToProperty(this, x => x.IsWorkPriceMultiplied);

			_totalPrice = this.WhenAnyValue(
					a => a.MaterialsPrice,
					b => b.WorkUnitsPrice,
					c => c.Product.MaterialsMultiplier,
					d => d.Product.WorkMultiplier,
					e => e.Product.ChargeForMaterials,
					f => f.Product.ChargeForWork,
				(mPrice, wPrice, mMultiplier, wMultiplier, mCharge, wCharge) => (mCharge ? mPrice * (mMultiplier == 0 ? 1 : mMultiplier) : 0) + (wCharge ? wPrice * (wMultiplier == 0 ? 1 : wMultiplier) : 0))
				.ToProperty(this, x => x.TotalPrice);
		}



        public ReactiveCommand Add => _add;

        public ReactiveCommand Edit => _edit;

        public ReactiveCommand ToggleChargeForMaterials => _toggleChargeForMaterials;

		public ReactiveCommand ToggleChargeForWork => _toggleChargeForWork;

		public ReactiveCommand SetMaterialsMultiplier => _setMaterialsMultiplier;

		public ReactiveCommand SetWorkMultiplier => _setWorkMultiplier;

		public Product Product => _product;

		public IRealmCollection<ProductMaterial> Materials => _product.Materials.AsRealmCollection();

		public IRealmCollection<WorkUnit> WorkUnits => _product.WorkUnits.AsRealmCollection();

		public double MaterialsPrice => _materialsPrice.Value;

		public bool IsMaterialsPriceMultiplied => _isMaterialsPriceMultiplied.Value;

		public double WorkUnitsPrice => _workUnitsPrice.Value;

		public bool IsWorkPriceMultiplied => _isWorkPriceMultiplied.Value;

		public double TotalPrice => _totalPrice.Value;



        private readonly ReactiveCommand<Unit, Unit> _add;
        private readonly ReactiveCommand<Unit, Unit> _edit;
        private readonly ReactiveCommand<Unit, Unit> _toggleChargeForMaterials;
		private readonly ReactiveCommand<Unit, Unit> _toggleChargeForWork;
		private readonly ReactiveCommand<double, Unit> _setMaterialsMultiplier;
		private readonly ReactiveCommand<double, Unit> _setWorkMultiplier;
		private readonly Product _product;
		private readonly ObservableAsPropertyHelper<double> _materialsPrice;
		private readonly ObservableAsPropertyHelper<bool> _isMaterialsPriceMultiplied;
		private readonly ObservableAsPropertyHelper<double> _workUnitsPrice;
		private readonly ObservableAsPropertyHelper<bool> _isWorkPriceMultiplied;
		private readonly ObservableAsPropertyHelper<double> _totalPrice;
	}
}
