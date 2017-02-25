using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.NavigationService;
using StitchCalc.Services.SettingsServices;
using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels
{
    public class WorkUnitFormViewViewModel : ViewModelBase, INavigable
    {
        public WorkUnitFormViewViewModel()
        {
            _save = ReactiveCommand.Create(
                () => SaveImpl(),
                this.WhenAnyValue(x => x.Name, y => y.Minutes, z => z.Charge, (x, y, z) =>
                !string.IsNullOrWhiteSpace(x)
                && !string.IsNullOrWhiteSpace(y)
                && !string.IsNullOrWhiteSpace(z)
                && y.IsValidDouble()
                && z.IsValidDouble()));
        }

        public ReactiveCommand Save => _save;

        public string PageTitle
        {
            get { return _pageTitle; }
            set { this.RaiseAndSetIfChanged(ref _pageTitle, value); }
        }

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



        public Task OnNavigatedTo(object parameter, NavigationDirection direction)
        {
            if (parameter is Guid)
            {
                _product = DBService.GetSingle<Product>((Guid)parameter);

                PageTitle = "Add Work";
                Charge = SettingsService.Current.DefaultHourlyCharge.ToString();
            }
            else
            {
                var p = (Tuple<Guid, Guid>)parameter;
                _product = DBService.GetSingle<Product>(p.Item1);
                _workUnit = DBService.GetSingle<WorkUnit>(p.Item2);

                PageTitle = "Edit Work";
                Name = _workUnit.Name;
                Minutes = _workUnit.Minutes.ToString();
				Charge = (_workUnit.Charge * 60).ToString();
            }

            return Task.CompletedTask;
        }

        public Task OnNavigatingFrom() => Task.CompletedTask;



        private async void SaveImpl()
        {
            var workUnit = new WorkUnit
            {
                Product = _product,
                Name = Name,
                Minutes = int.Parse(Minutes),
				Charge = double.Parse(Charge) / 60
            };
            if (_workUnit != null) workUnit.Id = _workUnit.Id;

            DBService.Write(realm => realm.Add(workUnit,true));

            await NavigationService.Current.GoBack();
        }



        private readonly ReactiveCommand<Unit, Unit> _save;
        private string _pageTitle;
        private string _name;
        private string _minutes;
        private string _charge;
        private Product _product;
        private WorkUnit _workUnit;
    }
}
