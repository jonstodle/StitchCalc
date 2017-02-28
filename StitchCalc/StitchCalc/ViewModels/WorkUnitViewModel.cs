using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services;
using StitchCalc.Views;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace StitchCalc.ViewModels
{
    public class WorkUnitViewModel : ViewModelBase
    {
        public WorkUnitViewModel(WorkUnit workUnit)
        {
            _workUnit = workUnit;

            _edit = ReactiveCommand.CreateFromObservable(() => Observable.FromAsync(() => NavigationService.NavigateTo(new WorkUnitFormView(new WorkUnitFormViewModel(_workUnit.Product, _workUnit)))));

			_delete = ReactiveCommand.Create(() => DBService.Write(realm => realm.Remove(_workUnit)));

			_totalCharge = this.WhenAnyValue(
				x => x.WorkUnit.Charge,
				y => y.WorkUnit.Minutes,
				(x, y) => x * y)
							   .ToProperty(this, x => x.TotalCharge);
        }



        public ReactiveCommand Edit => _edit;

		public ReactiveCommand Delete => _delete;

        public WorkUnit WorkUnit => _workUnit;

		public double TotalCharge => _totalCharge.Value;



		private readonly ReactiveCommand<Unit, Unit> _edit;
        private readonly ReactiveCommand<Unit, Unit> _delete;
        private readonly WorkUnit _workUnit;
		private readonly ObservableAsPropertyHelper<double> _totalCharge;
    }
}
