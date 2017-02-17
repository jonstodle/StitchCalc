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
    public class HomeViewViewModel : ViewModelBase, INavigable
    {
        public HomeViewViewModel()
        {
            _products = DBService.Get<Product, string>(x => x.Name);

            _navigateToProductFormPage = ReactiveCommand.CreateFromTask(() => NavigationService.Current.NavigateTo<ProductFormView>());

            _navigateToProductPage = ReactiveCommand.CreateFromTask(x => NavigationService.Current.NavigateTo<ProductView>(_selectedProduct.Id));

            _collectionView = Observable.Merge(
                    this.WhenAnyValue(x => x.SearchTerm),
                    _products.Changed().Select(_ => ""))
                .Throttle(TimeSpan.FromMilliseconds(500), RxApp.TaskpoolScheduler)
                .Select(x => CreateFilteredList(x))
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, x => x.CollectionView);
        }



        public IRealmCollection<Product> Products => _products;

        public IRealmCollection<Product> CollectionView => _collectionView.Value;

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



        private IRealmCollection<Product> CreateFilteredList(string searchString)
        {
            if (!searchString.HasValue()) return _products;
            else return DBService.Get<Product, string>(x => x.Name.ToLowerInvariant().Contains(searchString.ToLowerInvariant()), x => x.Name);
        }



        private readonly IRealmCollection<Product> _products;
        private readonly ObservableAsPropertyHelper<IRealmCollection<Product>> _collectionView;
        private readonly ReactiveCommand<Unit, Unit> _navigateToProductFormPage;
        private readonly ReactiveCommand<Unit, Unit> _navigateToProductPage;
        private string _searchTerm;
        private Product _selectedProduct;
    }
}
