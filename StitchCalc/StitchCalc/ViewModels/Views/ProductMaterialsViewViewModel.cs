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
		readonly ReactiveCommand<Unit, Unit> navigateToMaterialFormView;
		readonly ReactiveCommand<EventPattern<ItemTappedEventArgs>, Unit> edit;
		ProductViewModel product;

		public ProductMaterialsViewViewModel()
		{
			navigateToMaterialFormView = ReactiveCommand.CreateFromTask(() => NavigationService.Current.NavigateTo<ProductMaterialFormView>(product.Model.Id));

			edit = ReactiveCommand.CreateFromTask<EventPattern<ItemTappedEventArgs>, Unit>(async x => { await NavigationService.Current.NavigateTo<ProductMaterialFormView>(Tuple.Create(product.Model.Id, (x.EventArgs.Item as ProductMaterialViewModel)?.Model.Id)); return Unit.Default; });
		}

		public ReactiveCommand NavigateToProductMaterialFormView => navigateToMaterialFormView;

		public ReactiveCommand<EventPattern<ItemTappedEventArgs>, Unit> Edit => edit;

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
