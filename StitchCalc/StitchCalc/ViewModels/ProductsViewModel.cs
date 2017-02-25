using ReactiveUI;
using StitchCalc.Services.NavigationService;
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

namespace StitchCalc.ViewModels
{
    public class ProductsViewModel : ViewModelBase, INavigable
    {
        public ProductsViewModel()
        {
            _products = DBService.GetOrderedList<Product, string>(x => x.Name);

            _navigateToProductFormPage = ReactiveCommand.CreateFromTask(() => NavigationService.Current.NavigateTo<ProductFormView>());

            _navigateToProductPage = ReactiveCommand.CreateFromTask(x => NavigationService.Current.NavigateTo<ProductView>(_selectedProduct.Id));

			_productsView = Observable.Merge(
                    this.WhenAnyValue(x => x.SearchTerm),
					_products.CollectionChanges().Select(_ => SearchTerm))
                .Throttle(TimeSpan.FromMilliseconds(500), RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
				.Select(x => CreateFilteredList(x))
                .ToProperty(this, x => x.ProductsView);
        }



        public IRealmCollection<Product> Products => _products;

		public IReactiveDerivedList<ProductViewModel> ProductsView => _productsView.Value;

        public ReactiveCommand NavigateToProductFormPage => _navigateToProductFormPage;

        public ReactiveCommand NavigateToProductPage => _navigateToProductPage;

        public string SearchTerm
        {
            get { return _searchTerm; }
            set { this.RaiseAndSetIfChanged(ref _searchTerm, value); }
        }

        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set { this.RaiseAndSetIfChanged(ref _selectedProduct, value); }
        }



        public Task OnNavigatedTo(object parameter, NavigationDirection direction) => Task.CompletedTask;

        public Task OnNavigatingFrom() => Task.CompletedTask;



		private IReactiveDerivedList<ProductViewModel> CreateFilteredList(string searchString)
        {
			if (!searchString.HasValue()) return _products.CreateDerivedCollection(x => new ProductViewModel(x));
			else return DBService.GetFilteredList<Product, string>(x => x.Name.ToLowerInvariant().Contains(searchString.ToLowerInvariant()), x => x.Name).CreateDerivedCollection(x => new ProductViewModel(x));
        }



        private readonly IRealmCollection<Product> _products;
		private readonly ObservableAsPropertyHelper<IReactiveDerivedList<ProductViewModel>> _productsView;
        private readonly ReactiveCommand<Unit, Unit> _navigateToProductFormPage;
        private readonly ReactiveCommand<Unit, Unit> _navigateToProductPage;
        private string _searchTerm;
        private Product _selectedProduct;
    }
}
