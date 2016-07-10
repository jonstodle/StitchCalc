using ReactiveUI;
using StitchCalc.Services.NavigationService;
using StitchCalc.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StitchCalc.ViewModels.Views
{
	public class ProductViewViewModel : ViewModelBase, INavigable
	{
		readonly ReactiveList<Page> pages;

		public ProductViewViewModel()
		{
			pages = new ReactiveList<Page>();
			pages.Add(new ProductSummaryView());
			pages.Add(new ProductMaterialsView());
			pages.Add(new ProductWorkUnitsView());
		}

		public ReactiveList<Page> Pages => pages;

		public Task OnNavigatedTo(object parameter, NavigationDirection direction) => Task.CompletedTask;

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
