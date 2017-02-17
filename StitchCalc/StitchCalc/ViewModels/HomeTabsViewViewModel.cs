using ReactiveUI;
using StitchCalc.Services.NavigationService;
using StitchCalc.Views;
using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace StitchCalc.ViewModels
{
	public class HomeTabsViewViewModel : ViewModelBase, INavigable
	{
		readonly ReactiveList<Page> pages;

		public HomeTabsViewViewModel()
		{
			pages = new ReactiveList<Page>();
			pages.Add(new HomeView());
			pages.Add(new MaterialsView());
			pages.Add(new SettingsView());

			pages
				.Changed
				.Throttle(TimeSpan.FromMilliseconds(10))
				.Subscribe(_ => NavigateToPages(null, NavigationDirection.Forwards));
		}

		public ReactiveList<Page> Pages => pages;

		async void NavigateToPages(object parameter, NavigationDirection direction)
		{
			foreach (var page in pages)
			{
				await((page as IViewFor).ViewModel as INavigable).OnNavigatedTo(parameter, direction);
			}
		}

		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			NavigateToPages(parameter, direction);
			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
