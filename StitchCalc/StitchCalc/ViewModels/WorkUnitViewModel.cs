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
        }



        public ReactiveCommand Edit => _edit;

        public WorkUnit WorkUnit => _workUnit;



        private readonly ReactiveCommand<Unit, Unit> _edit;
        private readonly WorkUnit _workUnit;
    }
}
