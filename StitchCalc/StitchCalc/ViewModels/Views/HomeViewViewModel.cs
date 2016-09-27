using ReactiveUI;
using StitchCalc.Services.DataServices;
using StitchCalc.Services.NavigationService;
using StitchCalc.ViewModels.Models;
using StitchCalc.Views;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StitchCalc.ViewModels.Views
{
	public class HomeViewViewModel : ViewModelBase, INavigable
	{
		readonly IReactiveDerivedList<ProductViewModel> products;
		readonly ObservableAsPropertyHelper<IReactiveDerivedList<ProductViewModel>> collectionView;
		readonly ReactiveCommand<Unit, Unit> navigateToProductFormPage;
		readonly ReactiveCommand<EventPattern<ItemTappedEventArgs>, Unit> navigateToProductPage;
		string searchTerm;

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

			navigateToProductPage = ReactiveCommand.CreateFromTask<EventPattern<ItemTappedEventArgs>, Unit>(async x => { await NavigationService.Current.NavigateTo<ProductView>((x.EventArgs.Item as ProductViewModel)?.Model.Id); return Unit.Default; });
		}

		public IReactiveDerivedList<ProductViewModel> Products => products;

		public IReactiveDerivedList<ProductViewModel> CollectionView => collectionView.Value;

		public ReactiveCommand NavigateToProductFormPage => navigateToProductFormPage;

		public ReactiveCommand<EventPattern<ItemTappedEventArgs>, Unit> NavigateToProductPage => navigateToProductPage;

		public string SearchTerm
		{
			get { return searchTerm; }
			set { this.RaiseAndSetIfChanged(ref searchTerm, value); }
		}



		private IReactiveDerivedList<ProductViewModel> CreateDerivedList(string searchString)
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
