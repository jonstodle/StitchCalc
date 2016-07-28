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
		readonly ReactiveCommand<object> navigateToWorkUnitFormView;
		readonly ReactiveCommand<object> edit;
		ProductViewModel product;

		public ProductWorkUnitsViewViewModel()
		{
			navigateToWorkUnitFormView = ReactiveCommand.Create();
			navigateToWorkUnitFormView
				.Subscribe(async _ => await NavigationService.Current.NavigateTo<WorkUnitFormView>(product.Model.Id));

			edit = ReactiveCommand.Create();
			edit
				.Cast<EventPattern<ItemTappedEventArgs>>()
				.Select(x => x.EventArgs.Item)
				.Cast<WorkUnitViewModel>()
				.Subscribe(async item => await NavigationService.Current.NavigateTo<WorkUnitFormView>(Tuple.Create(product.Model.Id, item.Model.Id)));
		}

		public ReactiveCommand<object> NavigateToWorkUnitFormView => navigateToWorkUnitFormView;

		public ReactiveCommand<object> Edit => edit;

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
