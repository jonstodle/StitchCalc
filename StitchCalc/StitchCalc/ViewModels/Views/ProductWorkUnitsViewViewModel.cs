using ReactiveUI;
using StitchCalc.Services.DataServices;
using StitchCalc.Services.NavigationService;
using StitchCalc.ViewModels.Models;
using StitchCalc.Views;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels.Views
{
	public class ProductWorkUnitsViewViewModel : ViewModelBase, INavigable
	{
		readonly ReactiveCommand<object> navigateToWorkUnitFormView;
		readonly ReactiveCommand<object> edit;
		ProductViewModel product;
		object selectedWorkUnit;

		public ProductWorkUnitsViewViewModel()
		{
			navigateToWorkUnitFormView = ReactiveCommand.Create();
			navigateToWorkUnitFormView
				.Subscribe(async _ => await NavigationService.Current.NavigateTo<WorkUnitFormView>(product.Model.Id));

			edit = ReactiveCommand.Create();
			edit
				.Select(x => selectedWorkUnit)
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

		public object SelectedWorkUnit
		{
			get { return selectedWorkUnit; }
			set { this.RaiseAndSetIfChanged(ref selectedWorkUnit, value); }
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
