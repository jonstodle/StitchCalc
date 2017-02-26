using ReactiveUI;
using StitchCalc.Views;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using StitchCalc.Models;
using Realms;
using System.Collections.Specialized;
using StitchCalc.Services;

namespace StitchCalc.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        public ProductsViewModel()
        {
            _products = DBService.GetOrderedList<Product, string>(x => x.Name);

            _addProduct = ReactiveCommand.CreateFromObservable(() => Observable.FromAsync(() => NavigationService.NavigateTo(new ProductFormView(new ProductFormViewModel()))));

            _showProduct = ReactiveCommand.CreateFromTask(x => NavigationService.NavigateTo(new ProductView(_selectedProduct)));

			_productsView = Observable.Merge(
                    this.WhenAnyValue(x => x.SearchTerm),
					_products.CollectionChanges().Select(_ => SearchTerm))
                .Throttle(TimeSpan.FromMilliseconds(500), RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
				.Select(x => CreateFilteredList(x))
                .ToProperty(this, x => x.ProductsView);
        }



        public ReactiveCommand NavigateToProductFormPage => _addProduct;

        public ReactiveCommand NavigateToProductPage => _showProduct;

        public IRealmCollection<Product> Products => _products;

        public IReactiveDerivedList<ProductViewModel> ProductsView => _productsView.Value;

        public string SearchTerm
        {
            get { return _searchTerm; }
            set { this.RaiseAndSetIfChanged(ref _searchTerm, value); }
        }

        public ProductViewModel SelectedProduct
        {
            get { return _selectedProduct; }
            set { this.RaiseAndSetIfChanged(ref _selectedProduct, value); }
        }



		private IReactiveDerivedList<ProductViewModel> CreateFilteredList(string searchString)
        {
			if (!searchString.HasValue()) return _products.CreateDerivedCollection(x => new ProductViewModel(x));
			else return DBService.GetFilteredList<Product, string>(x => x.Name.ToLowerInvariant().Contains(searchString.ToLowerInvariant()), x => x.Name).CreateDerivedCollection(x => new ProductViewModel(x));
        }



        private readonly ReactiveCommand<Unit, Unit> _addProduct;
        private readonly ReactiveCommand<Unit, Unit> _showProduct;
        private readonly IRealmCollection<Product> _products;
		private readonly ObservableAsPropertyHelper<IReactiveDerivedList<ProductViewModel>> _productsView;
        private string _searchTerm;
        private ProductViewModel _selectedProduct;
    }
}
