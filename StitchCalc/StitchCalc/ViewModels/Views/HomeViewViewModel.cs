using ReactiveUI;
using StitchCalc.Services.DataServices;
using StitchCalc.Services.NavigationService;
using StitchCalc.ViewModels.Models;
using StitchCalc.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels.Views
{
	public class HomeViewViewModel : ViewModelBase, INavigable
	{
		readonly IReactiveDerivedList<ProductViewModel> products;
		readonly ObservableAsPropertyHelper<List<ProductViewModel>> collectionView;
		readonly ReactiveCommand<object> navigateToProductFormPage;
		readonly ReactiveCommand<object> navigateToProductPage;
		string searchTerm;

		public HomeViewViewModel()
		{
			products = DataService.Current.GetProducts();

			products
				.Changed
				.Select(_ => products.OrderBy(x => x.Name).ToList())
				.ToProperty(this, x => x.CollectionView, out collectionView);

			navigateToProductFormPage = ReactiveCommand.Create();
			navigateToProductFormPage
				.Subscribe(async _ => await NavigationService.Current.NavigateTo<ProductFormView>());

			navigateToProductPage = ReactiveCommand.Create();
			navigateToProductPage
				.Subscribe(async _ => await NavigationService.Current.NavigateTo<ProductView>());
		}

		public List<ProductViewModel> CollectionView => collectionView.Value;

		public ReactiveCommand<object> NavigateToProductFormPage => navigateToProductFormPage;

		public ReactiveCommand<object> NavigateToProductPage => navigateToProductPage;

		public string SearchTerm
		{
			get { return searchTerm; }
			set { this.RaiseAndSetIfChanged(ref searchTerm, value); }
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction) => Task.CompletedTask;

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
