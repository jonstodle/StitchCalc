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
		public HomeTabsViewViewModel() { }

        public Task OnNavigatedTo(object parameter, NavigationDirection direction) => Task.CompletedTask;

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
