using ReactiveUI;
using StitchCalc.Services.NavigationService;
using StitchCalc.Views;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using StitchCalc.Models;

namespace StitchCalc.ViewModels
{
	public class ProductWorkUnitsViewViewModel : ViewModelBase, INavigable
	{
		public ProductWorkUnitsViewViewModel()
		{
			_navigateToWorkUnitFormView = ReactiveCommand.CreateFromTask(() => NavigationService.Current.NavigateTo<WorkUnitFormView>(_product.Id));

			_edit = ReactiveCommand.CreateFromTask(x => NavigationService.Current.NavigateTo<WorkUnitFormView>(Tuple.Create(_product.Id, _selectedWorkUnit.Id)));
		}

		public ReactiveCommand NavigateToWorkUnitFormView => _navigateToWorkUnitFormView;

		public ReactiveCommand Edit => _edit;

		public Product Product
		{
			get { return _product; }
			set { this.RaiseAndSetIfChanged(ref _product, value); }
		}

		public WorkUnit SelectedWorkUnit
		{
			get { return _selectedWorkUnit; }
			set { this.RaiseAndSetIfChanged(ref _selectedWorkUnit, value); }
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				Product = DBService.GetSingle<Product>((Guid)parameter);
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;



        private readonly ReactiveCommand<Unit, Unit> _navigateToWorkUnitFormView;
        private readonly ReactiveCommand<Unit, Unit> _edit;
        private Product _product;
        private WorkUnit _selectedWorkUnit;
    }
}
