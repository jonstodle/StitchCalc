using ReactiveUI;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StitchCalc.Services
{
	public static class NavigationService
	{
		public static void Init(INavigation navigation) => this.navigation = navigation;





        public static Task NavigateTo<TView>(TView view) where TView : Page, IViewFor => NavigateTo(view, false);

		public static Task NavigateToAndRemoveThis<TView>(TView view) where TView : Page, IViewFor => NavigateTo(view, true);

		private static async Task NavigateTo<TView>(TView view, bool removeCurrentPageFromBackStack) where TView : Page, IViewFor
		{
            var currentPage = navigation.NavigationStack.LastOrDefault();
            await navigation.PushAsync(view);
            if (removeCurrentPageFromBackStack) navigation.RemovePage(currentPage);
        }

		public static Task GoBack() => navigation.PopAsync();

        public static Task GoHome() => navigation.PopToRootAsync();



        private static INavigation navigation;
    }
}
