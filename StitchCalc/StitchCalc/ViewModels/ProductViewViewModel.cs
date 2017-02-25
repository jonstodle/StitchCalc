using ReactiveUI;
using StitchCalc.Services.NavigationService;
using StitchCalc.Views;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using StitchCalc.Models;
using System.Collections.Specialized;

namespace StitchCalc.ViewModels
{
	public class ProductViewViewModel : ViewModelBase, INavigable
	{
		public ProductViewViewModel()
		{
			_pages = new ReactiveList<Page>();
			_pages.Add(new ProductSummaryView());
			_pages.Add(new ProductMaterialsView());
			_pages.Add(new ProductWorkUnitsView());

			DBService.GetList<Product>()
				.CollectionChanges()
				.Select(_ => DBService.GetSingle<Product>(_product.Id))
				.Subscribe(newProduct => Product = newProduct);

			Observable.Merge(
				_pages.Changed.Select(_ => 0),
				this.WhenAnyValue(x=> x.Product).Where(x => x != null).Select(_ => 0))
				.Subscribe(_ => SetModelForChildViews());

			this
				.WhenAnyValue(x => x.Product, x => x?.Name)
				.ToProperty(this, x => x.PageTitle, out _pageTitle);
		}

		public ReactiveList<Page> Pages => _pages;

		public string PageTitle => _pageTitle.Value;

		public Product Product
		{
			get { return _product; }
			set { this.RaiseAndSetIfChanged(ref _product, value); }
		}

		public int SelectedPageIndex
		{
			get { return _selectedPageIndex; }
			set { this.RaiseAndSetIfChanged(ref _selectedPageIndex, value); }
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				Product = DBService.GetSingle<Product>((Guid)parameter);
			}
			else if (parameter is Tuple<Guid,int>)
			{
				var p = parameter as Tuple<Guid, int>;

				Product = DBService.GetSingle<Product>(p.Item1);
				SelectedPageIndex = p.Item2;
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;



        private void SetModelForChildViews()
        {
            foreach (var page in _pages)
            {
                ((page as IViewFor).ViewModel as INavigable).OnNavigatedTo(_product.Id, NavigationDirection.Forwards);
            }
        }



        private readonly ReactiveList<Page> _pages;
        private readonly ObservableAsPropertyHelper<string> _pageTitle;
        private Product _product;
        private int _selectedPageIndex;
    }
}
