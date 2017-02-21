using ReactiveUI;
using StitchCalc.Services.NavigationService;
using StitchCalc.Views;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using StitchCalc.Models;
using Realms;
using System.Collections.Specialized;
using System.Linq;

namespace StitchCalc.ViewModels
{
	public class ProductWorkUnitsViewViewModel : ViewModelBase, INavigable
	{
		public ProductWorkUnitsViewViewModel()
		{
			_navigateToWorkUnitFormView = ReactiveCommand.CreateFromTask(() => NavigationService.Current.NavigateTo<WorkUnitFormView>(_product.Id));

			_edit = ReactiveCommand.CreateFromTask(x => NavigationService.Current.NavigateTo<WorkUnitFormView>(Tuple.Create(_product.Id, _selectedWorkUnit.Id)));

            _workPrice = this.WhenAnyValue(x => x.Product)
                .WhereNotNull()
                .SelectMany(x => x.WorkUnits.AsRealmCollection().Changed().Select(_ => x.WorkUnits.ToList()).StartWith(x.WorkUnits.ToList()))
                .Select(x => x.Sum(y => y.Charge * y.Minutes))
                .ToProperty(this, x => x.WorkPrice);
		}

		public ReactiveCommand NavigateToWorkUnitFormView => _navigateToWorkUnitFormView;

		public ReactiveCommand Edit => _edit;

        public double WorkPrice => _workPrice.Value;

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
        private readonly ObservableAsPropertyHelper<double> _workPrice;
        private Product _product;
        private WorkUnit _selectedWorkUnit;
    }
}
