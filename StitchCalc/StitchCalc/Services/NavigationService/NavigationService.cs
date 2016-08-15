using ReactiveUI;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StitchCalc.Services.NavigationService
{
	public class NavigationService
	{
		public static NavigationService Current { get; private set; }



		INavigation navigation;
		IViewFor currentPage => navigation.NavigationStack.LastOrDefault() as IViewFor;
		IViewFor previousPage => navigation.NavigationStack.Reverse().Skip(1).FirstOrDefault() as IViewFor;

		public NavigationService(INavigation navigation)
		{
			this.navigation = navigation;
			Current = Current ?? this;
		}

		public Task NavigateTo<TView>(object parameter = null) where TView : class, IViewFor, new() => NavigateTo<TView>(parameter, false);

		public Task NavigateToAndRemoveThis<TView>(object parameter = null) where TView : class, IViewFor, new() => NavigateTo<TView>(parameter, true);

		private async Task NavigateTo<TView>(object parameter, bool removeCurrentPageFromBackStack) where TView : class, IViewFor, new()
		{
			await(currentPage.ViewModel as INavigable)?.OnNavigatingFrom();

			var newPage = new TView();
			await(newPage.ViewModel as INavigable).OnNavigatedTo(parameter, NavigationDirection.Forwards);

			if (removeCurrentPageFromBackStack && currentPage != null)
			{
				navigation.InsertPageBefore(newPage as Page, navigation.NavigationStack.Last());
				await navigation.PopAsync();
			}
			else { await navigation.PushAsync(newPage as Page); }
		}

		public async Task GoBack()
		{
			await (currentPage.ViewModel as INavigable)?.OnNavigatingFrom();

			await (previousPage.ViewModel as INavigable)?.OnNavigatedTo(null, NavigationDirection.Backwards);

			await navigation.PopAsync();
		}

		public async Task GoHome()
		{
			await navigation.PopToRootAsync();
		}
	}
}
