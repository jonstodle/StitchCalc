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
	public class ProductWorkUnitsViewViewModel : ViewModelBase, INavigable
	{
		readonly ReactiveCommand<Unit, Unit> navigateToWorkUnitFormView;
		readonly ReactiveCommand<EventPattern<ItemTappedEventArgs>, Unit> edit;
		ProductViewModel product;

		public ProductWorkUnitsViewViewModel()
		{
			navigateToWorkUnitFormView = ReactiveCommand.CreateFromTask(() => NavigationService.Current.NavigateTo<WorkUnitFormView>(product.Model.Id));

			edit = ReactiveCommand.CreateFromTask<EventPattern<ItemTappedEventArgs>, Unit>(async x => { await NavigationService.Current.NavigateTo<WorkUnitFormView>(Tuple.Create(product.Model.Id, (x.EventArgs.Item as WorkUnitViewModel)?.Model.Id)); return Unit.Default; });
		}

		public ReactiveCommand NavigateToWorkUnitFormView => navigateToWorkUnitFormView;

		public ReactiveCommand<EventPattern<ItemTappedEventArgs>, Unit> Edit => edit;

		public ProductViewModel Model
		{
			get { return product; }
			set { this.RaiseAndSetIfChanged(ref product, value); }
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				Model = DataService.Current.GetProduct((Guid)parameter);
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
