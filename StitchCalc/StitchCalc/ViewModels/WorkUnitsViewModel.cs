using ReactiveUI;
using Realms;
using StitchCalc.Models;
using StitchCalc.Services;
using StitchCalc.Views;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace StitchCalc.ViewModels
{
	public class WorkUnitsViewModel : ViewModelBase
	{
		public WorkUnitsViewModel(Product product)
		{
			_product = product;

			_addWorkUnit = ReactiveCommand.CreateFromObservable(() => Observable.FromAsync(() => NavigationService.NavigateTo(new WorkUnitFormView(new WorkUnitFormViewModel(_product)))));

			_workUnits = _product.WorkUnits.OrderBy(x => x.Name).AsRealmCollection();

			_workUnitsView = _workUnits.CreateDerivedCollection(x => new WorkUnitViewModel(x));

			_workPrice = _workUnits
				.CollectionChanges()
			    .ToSignal()
			    .StartWith(Unit.Default)
				.Select(_ => _workUnits.Sum(x => x.Charge * x.Minutes))
				.ToProperty(this, x => x.WorkPrice);
		}



		public ReactiveCommand AddWorkUnit => _addWorkUnit;

		public IRealmCollection<WorkUnit> WorkUnits => _workUnits;

		public IReactiveDerivedList<WorkUnitViewModel> WorkUnitsView => _workUnitsView;

		public Product Product => _product;

		public double WorkPrice => _workPrice.Value;

		public WorkUnitViewModel SelectedWorkUnit
		{
			get { return _selectedWorkUnit; }
			set { this.RaiseAndSetIfChanged(ref _selectedWorkUnit, value); }
		}



		private readonly ReactiveCommand<Unit, Unit> _addWorkUnit;
		private readonly IRealmCollection<WorkUnit> _workUnits;
		private readonly IReactiveDerivedList<WorkUnitViewModel> _workUnitsView;
		private readonly Product _product;
		private readonly ObservableAsPropertyHelper<double> _workPrice;
		private WorkUnitViewModel _selectedWorkUnit;
	}
}
