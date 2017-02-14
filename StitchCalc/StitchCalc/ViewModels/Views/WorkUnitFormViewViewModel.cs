using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.DataServices;
using StitchCalc.Services.NavigationService;
using StitchCalc.Services.SettingsServices;
using StitchCalc.ViewModels.Models;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels.Views
{
	public class WorkUnitFormViewViewModel : ViewViewModelBase, INavigable
	{
		readonly ReactiveCommand<Unit, Unit> save;
		string pageTitle;
		string name;
		string minutes;
		string charge;
		ProductViewModel product;
		WorkUnitViewModel workUnit;


		public WorkUnitFormViewViewModel()
		{
			save = ReactiveCommand.Create(
				() => SaveImpl(),
				this.WhenAnyValue(x => x.Name, y => y.Minutes, z => z.Charge, (x, y, z) =>
				!string.IsNullOrWhiteSpace(x)
				&& !string.IsNullOrWhiteSpace(y)
				&& !string.IsNullOrWhiteSpace(z)
				&& y.IsValidDouble()
				&& z.IsValidDouble()));
		}

		public ReactiveCommand Save => save;

		public string PageTitle
		{
			get { return pageTitle; }
			set { this.RaiseAndSetIfChanged(ref pageTitle, value); }
		}

		public string Name
		{
			get { return name; }
			set { this.RaiseAndSetIfChanged(ref name, value); }
		}

		public string Minutes
		{
			get { return minutes; }
			set { this.RaiseAndSetIfChanged(ref minutes, value); }
		}

		public string Charge
		{
			get { return charge; }
			set { this.RaiseAndSetIfChanged(ref charge, value); }
		}

		private async void SaveImpl()
		{
			var wrknt = new WorkUnit
			{
				Id = workUnit?.Model.Id ?? default(Guid),
				ProductId = product.Model.Id,
				Name = Name,
				Minutes = int.Parse(Minutes),
				Charge = double.Parse(Charge) / 60
			};

			if (workUnit == null) { DataService.Current.Add(wrknt); }
			else { DataService.Current.Update(wrknt); }

			await NavigationService.Current.GoBack();
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				product = DataService.Current.GetProduct((Guid)parameter);

				PageTitle = "Add Work";
				Charge = SettingsService.Current.DefaultHourlyCharge.ToString();
			}
			else
			{
				var p = (Tuple<Guid, Guid>)parameter;
				product = DataService.Current.GetProduct(p.Item1);
				workUnit = DataService.Current.GetWorkUnit(p.Item2);

				PageTitle = "Edit Work";
				Name = workUnit.Name;
				Minutes = workUnit.Minutes.ToString();
				Charge = workUnit.ChargePerHour.ToString();
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
