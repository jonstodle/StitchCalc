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

		public async Task NavigateTo<TView>(object parameter = null) where TView : class, IViewFor, new()
		{
			if (currentPage?.ViewModel is INavigable){await (currentPage.ViewModel as INavigable).OnNavigatingFrom(); }

			currentPage = new TView();
			if (currentPage.ViewModel is INavigable) { await (currentPage.ViewModel as INavigable).OnNavigatedTo(parameter, NavigationDirection.Forwards); }

			await navigation.PushAsync(currentPage as Page);
		}

		public async Task GoBack()
		{
			await (currentPage.ViewModel as INavigable)?.OnNavigatingFrom();

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
