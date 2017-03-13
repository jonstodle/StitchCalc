using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System;

namespace StitchCalc.ViewModels
{
    public class WorkUnitFormViewModel : ViewModelBase
    {
        public WorkUnitFormViewModel(Product product, WorkUnit workUnit = null)
        {
            _product = product;
            _workUnit = workUnit;
            Name = _workUnit?.Name;
            Minutes = _workUnit?.Minutes.ToString();
            Charge = _workUnit != null ? (_workUnit.Charge * 60).ToString() : SettingsService.Current.DefaultHourlyCharge.ToString();

            _save = ReactiveCommand.CreateFromObservable(
                () => SaveImpl(),
                this.WhenAnyValue(x => x.Name, y => y.Minutes, z => z.Charge, (x, y, z) =>
                !string.IsNullOrWhiteSpace(x)
                && !string.IsNullOrWhiteSpace(y)
                && !string.IsNullOrWhiteSpace(z)
                && y.IsValidDouble()
                && z.IsValidDouble()));
        }



        public ReactiveCommand Save => _save;

        public string PageTitle => _workUnit == null ? "Add Work" : "Edit Work";

        public string Name
        {
            get { return _name; }
            set { this.RaiseAndSetIfChanged(ref _name, value); }
        }

        public string Minutes
        {
            get { return _minutes; }
            set { this.RaiseAndSetIfChanged(ref _minutes, value); }
        }

        public string Charge
        {
            get { return _charge; }
            set { this.RaiseAndSetIfChanged(ref _charge, value); }
        }



        private IObservable<Unit> SaveImpl() => Observable.FromAsync(async () =>
        {
            var workUnit = new WorkUnit
            {
                Product = _product,
                Name = Name,
                Minutes = int.Parse(Minutes),
                Charge = double.Parse(Charge) / 60
            };
            if (_workUnit != null) workUnit.Id = _workUnit.Id;

            DBService.Write(realm => realm.Add(workUnit, true));

            await NavigationService.GoBack();
        });



        private readonly ReactiveCommand<Unit, Unit> _save;
        private string _name;
        private string _minutes;
        private string _charge;
        private Product _product;
        private WorkUnit _workUnit;
    }
}
