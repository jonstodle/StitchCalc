using ReactiveUI;
using StitchCalc.Views;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using StitchCalc.Models;
using Realms;
using System.Collections.Specialized;
using System.Linq;
using StitchCalc.Services;

namespace StitchCalc.ViewModels
{
	public class ProductMaterialsViewModel : ViewModelBase
	{
		public ProductMaterialsViewModel(Product product)
		{
            _product = product;

			_addProductMaterial = ReactiveCommand.CreateFromObservable(() => Observable.FromAsync(() => NavigationService.NavigateTo(new ProductMaterialFormView(new ProductMaterialFormViewModel(_product)))));

            _productMaterials = _product.Materials.OrderBy(x => x.Name).AsRealmCollection();

            _productMaterialsView = _productMaterials.CreateDerivedCollection(x => new ProductMaterialViewModel(x));

			_materialsPrice = _productMaterials
                .CollectionChanges()
				.Select(_ => _productMaterials.Sum(x => x.Price))
				.ToProperty(this, x => x.MaterialsPrice);
		}



		public ReactiveCommand AddProductMaterial => _addProductMaterial;

        public IRealmCollection<ProductMaterial> ProductMaterials => _productMaterials;

        public IReactiveDerivedList<ProductMaterialViewModel> ProductMaterialsView => _productMaterialsView;

        public Product Product => _product;

        public double MaterialsPrice => _materialsPrice.Value;

		public ProductMaterialViewModel SelectedProductMaterial
		{
			get { return _selectedProductMaterial; }
			set { this.RaiseAndSetIfChanged(ref _selectedProductMaterial, value); }
		}



		private readonly ReactiveCommand<Unit, Unit> _addProductMaterial;
        private readonly IRealmCollection<ProductMaterial> _productMaterials;
        private readonly IReactiveDerivedList<ProductMaterialViewModel> _productMaterialsView;
        private readonly Product _product;
		private readonly ObservableAsPropertyHelper<double> _materialsPrice;
		private ProductMaterialViewModel _selectedProductMaterial;
	}
}
