using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services;
using StitchCalc.Views;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace StitchCalc.ViewModels
{
    public class ProductMaterialViewModel : ViewModelBase
    {
        public ProductMaterialViewModel(ProductMaterial productMaterial)
        {
            _productMaterial = productMaterial;

            _edit = ReactiveCommand.CreateFromObservable(() => Observable.FromAsync(() => NavigationService.NavigateTo(new ProductMaterialFormView(new ProductMaterialFormViewModel(_productMaterial.Product, _productMaterial)))));

			_delete = ReactiveCommand.Create(() => DBService.Write(realm => realm.Remove(_productMaterial)));

			_price = this.WhenAnyValue(
				x => x.ProductMaterial.Price,
				y => y.ProductMaterial.Length,
				(x, y) => x * y)
						 .ToProperty(this, x => x.Price);
        }



        public ReactiveCommand Edit => _edit;

		public ReactiveCommand Delete => _delete;

        public ProductMaterial ProductMaterial => _productMaterial;

		public double Price => _price.Value;



		private readonly ReactiveCommand<Unit, Unit> _edit;
        private readonly ReactiveCommand<Unit, Unit> _delete;
        private readonly ProductMaterial _productMaterial;
		private readonly ObservableAsPropertyHelper<double> _price;
    }
}
