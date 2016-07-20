using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StitchCalc.Services.NavigationService
{
	public class NavigationService
	{
		public static NavigationService Current { get; private set; }



		INavigation navigation;
		IViewFor currentPage;

		public NavigationService(INavigation navigation)
		{
			this.navigation = navigation;
			Current = Current ?? this;
		}

		public Task NavigateTo<TView>(object parameter = null) where TView : class, IViewFor, new() => NavigateTo<TView>(parameter, false);

		public Task NavigateToAndRemoveThis<TView>(object parameter = null) where TView : class, IViewFor, new() => NavigateTo<TView>(parameter, true);

		private async Task NavigateTo<TView>(object parameter, bool removeCurrentPageFromBackStack) where TView : class, IViewFor, new()
		{
			if (currentPage?.ViewModel is INavigable) { await(currentPage.ViewModel as INavigable).OnNavigatingFrom(); }

			this.currentPage = new TView();
			if (this.currentPage.ViewModel is INavigable) { await(this.currentPage.ViewModel as INavigable).OnNavigatedTo(parameter, NavigationDirection.Forwards); }

			if (removeCurrentPageFromBackStack && currentPage != null)
			{
				navigation.InsertPageBefore(this.currentPage as Page, navigation.NavigationStack[navigation.NavigationStack.Count -1]);
				await navigation.PopAsync();
			}
			else { await navigation.PushAsync(this.currentPage as Page); }
		}

		public async Task GoBack()
		{
			await (currentPage.ViewModel as INavigable)?.OnNavigatingFrom();

			currentPage = navigation.NavigationStack[navigation.NavigationStack.Count - 1] as IViewFor;

			var navStack = navigation.NavigationStack;
			var prevPageIndex = navStack.Count - 2;
			if (prevPageIndex >= 0)
			{
				await ((navStack[prevPageIndex] as IViewFor)?.ViewModel as INavigable)?.OnNavigatedTo(null, NavigationDirection.Backwards);
			}

			await navigation.PopAsync();
		}
	}
}
