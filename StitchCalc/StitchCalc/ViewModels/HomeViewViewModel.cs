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

namespace StitchCalc.ViewModels
{
	public class HomeViewViewModel : ViewModelBase, INavigable
	{
		readonly IRealmCollection<Product> products;
		readonly ObservableAsPropertyHelper<IRealmCollection<Product>> collectionView;
		readonly ReactiveCommand<Unit, Unit> navigateToProductFormPage;
		readonly ReactiveCommand<Unit, Unit> navigateToProductPage;
		string searchTerm;
		object selectedProduct;

		public HomeViewViewModel()
		{
			products = DataService.Current.GetProducts();

			this
				.WhenAnyValue(x => x.SearchTerm)
				.Merge(products.Changed.Select(_ => ""))
				.Throttle(TimeSpan.FromMilliseconds(500), RxApp.TaskpoolScheduler)
				.Select(x => CreateDerivedList(x))
				.ObserveOn(RxApp.MainThreadScheduler)
				.ToProperty(this, x => x.CollectionView, out collectionView);

			navigateToProductFormPage = ReactiveCommand.CreateFromTask(() => NavigationService.Current.NavigateTo<ProductFormView>());

			navigateToProductPage = ReactiveCommand.CreateFromTask(x => NavigationService.Current.NavigateTo<ProductView>((selectedProduct as ProductViewModel).Model.Id));
		}

		public IRealmCollection<Product> Products => products;

		public IRealmCollection<Product> CollectionView => collectionView.Value;

		public ReactiveCommand NavigateToProductFormPage => navigateToProductFormPage;

		public ReactiveCommand NavigateToProductPage => navigateToProductPage;

		public string SearchTerm
		{
			get { return searchTerm; }
			set { this.RaiseAndSetIfChanged(ref searchTerm, value); }
		}

		public object SelectedProduct
		{
			get { return selectedProduct; }
			set { this.RaiseAndSetIfChanged(ref selectedProduct, value); }
		}



		private IRealmCollection<Product> CreateDerivedList(string searchString)
		{
			var orderFunc = new Func<ProductViewModel, ProductViewModel, int>((item1, item2) => item1.Name.CompareTo(item2.Name));

			if (string.IsNullOrWhiteSpace(searchString))
			{
				return Products.CreateDerivedCollection(x => x, orderer: orderFunc);
			}
			else
			{
				return Products.CreateDerivedCollection(x => x, x => x.Name.ToLowerInvariant().Contains(searchString.ToLowerInvariant()), orderFunc);
			}
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction) => Task.CompletedTask;

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
