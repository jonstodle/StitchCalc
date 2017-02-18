using ReactiveUI;
using StitchCalc.Services.NavigationService;
using StitchCalc.Views;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using StitchCalc.Models;
using Realms;
using System.Collections.Specialized;
using System.Linq;

namespace StitchCalc.ViewModels
{
	public class ProductSummaryViewViewModel : ViewModelBase, INavigable
	{
		public ProductSummaryViewViewModel()
		{
			_edit = ReactiveCommand.CreateFromTask(() => NavigationService.Current.NavigateTo<ProductFormView>(_product.Id));

            _toggleChargeForMaterials = ReactiveCommand.Create(() => DBService.Write(realm => _product.ChargeForMaterials = !_product.ChargeForMaterials));

            _setMaterialsMultiplier = ReactiveCommand.Create<double, Unit>(value => { DBService.Write(realm => _product.MaterialsMultiplier = value); return Unit.Default; });

            _toggleChargeForWork = ReactiveCommand.Create(() => DBService.Write(realm => _product.ChargeForWork = !_product.ChargeForWork));

            _setWorkMultiplier = ReactiveCommand.Create<double, Unit>(value => { DBService.Write(realm => _product.WorkMultiplier = value); return Unit.Default; });

            var productChanged = this.WhenAnyValue(x => x.Product).WhereNotNull();

            _materialsPrice = productChanged
                .SelectMany(x => x.Materials.AsRealmCollection().Changed().Select(_ => x.Materials.ToList()))
                .Select(x => x.Sum(y => y.Price))
                .ToProperty(this, x => x.MaterialsPrice);

            _isMaterialsPriceMultiplied = this.WhenAnyValue(x => x.Product.MaterialsMultiplier)
                .Select(x => x > 0)
                .ToProperty(this, x => x.IsMaterialsPriceMultiplied);

            _workPrice = productChanged
                .SelectMany(x => x.WorkUnits.AsRealmCollection().Changed().Select(_ => x.WorkUnits.ToList()))
                .Select(x => x.Sum(y => y.Charge * y.Minutes))
                .ToProperty(this, x => x.WorkPrice);

            _isWorkPriceMultiplied = this.WhenAnyValue(x => x.Product.WorkMultiplier)
                .Select(x => x > 0)
                .ToProperty(this, x => x.IsWorkPriceMultiplied);

            _totalPrice = this.WhenAnyValue(
                    a => a.MaterialsPrice,
                    b => b.WorkPrice,
                    c => c.Product.MaterialsMultiplier,
                    d => d.Product.WorkMultiplier,
                    e => e.Product.ChargeForMaterials,
                    f => f.Product.ChargeForWork,
                    (mPrice, wPrice, mMultiplier, wMultiplier, mCharge, wCharge) => (mCharge ? mPrice * (mMultiplier == 0 ? 1 : mMultiplier) : 0) + (wCharge ? wPrice * (wPrice == 0 ? 1 : wMultiplier) : 0))
                .ToProperty(this, x => x.TotalPrice);
        }



		public ReactiveCommand Edit => _edit;

        public ReactiveCommand ToggleChargeForMaterials => _toggleChargeForMaterials;

        public ReactiveCommand SetMaterialsMultiplier => _setMaterialsMultiplier;

        public ReactiveCommand ToggleChargeForWork => _toggleChargeForWork;

        public ReactiveCommand SetWorkMultiplier => _setWorkMultiplier;

        public double MaterialsPrice => _materialsPrice.Value;

        public bool IsMaterialsPriceMultiplied => _isMaterialsPriceMultiplied.Value;

        public double WorkPrice => _workPrice.Value;

        public bool IsWorkPriceMultiplied => _isWorkPriceMultiplied.Value;

		public double TotalPrice => _totalPrice.Value;

        public Product Product
		{
			get { return _product; }
			set { this.RaiseAndSetIfChanged(ref _product, value); }
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				Product = DBService.GetSingle<Product>((Guid)parameter);
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;



        private readonly ReactiveCommand<Unit, Unit> _edit;
        private readonly ReactiveCommand<Unit, Unit> _toggleChargeForMaterials;
        private readonly ReactiveCommand<double, Unit> _setMaterialsMultiplier;
        private readonly ReactiveCommand<Unit, Unit> _toggleChargeForWork;
        private readonly ReactiveCommand<double, Unit> _setWorkMultiplier;
        private readonly ObservableAsPropertyHelper<double> _materialsPrice;
        private readonly ObservableAsPropertyHelper<bool> _isMaterialsPriceMultiplied;
        private readonly ObservableAsPropertyHelper<double> _workPrice;
        private readonly ObservableAsPropertyHelper<bool> _isWorkPriceMultiplied;
        private readonly ObservableAsPropertyHelper<double> _totalPrice;
        private Product _product;
    }
}
