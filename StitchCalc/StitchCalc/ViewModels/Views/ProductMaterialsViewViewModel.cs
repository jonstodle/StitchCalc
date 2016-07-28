using ReactiveUI;
using StitchCalc.Services.DataServices;
using StitchCalc.Services.NavigationService;
using StitchCalc.ViewModels.Models;
using StitchCalc.Views;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StitchCalc.ViewModels.Views
{
	public class ProductMaterialsViewViewModel : ViewModelBase, INavigable
	{
		readonly ReactiveCommand<object> navigateToMaterialFormView;
		readonly ReactiveCommand<object> edit;
		ProductViewModel product;

		public ProductMaterialsViewViewModel()
		{
			navigateToMaterialFormView = ReactiveCommand.Create();
			navigateToMaterialFormView
				.Subscribe(async _ => await NavigationService.Current.NavigateTo<ProductMaterialFormView>(product.Model.Id));

			edit = ReactiveCommand.Create();
			edit
				.Cast<EventPattern<ItemTappedEventArgs>>()
				.Select(x => x.EventArgs.Item)
				.Cast<ProductMaterialViewModel>()
				.Subscribe(async item => await NavigationService.Current.NavigateTo<ProductMaterialFormView>(Tuple.Create(product.Model.Id, item.Model.Id)));
		}

		public ReactiveCommand<object> NavigateToProductMaterialFormView => navigateToMaterialFormView;

		public ReactiveCommand<object> Edit => edit;

		public ProductViewModel Product
		{
			get { return product; }
			set { this.RaiseAndSetIfChanged(ref product, value); }
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				Product = DataService.Current.GetProduct((Guid)parameter);
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
