using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.DataServices;
using StitchCalc.Services.NavigationService;
using StitchCalc.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels.Views
{
	public class WorkUnitFormViewViewModel : ViewModelBase, INavigable
	{
		readonly ReactiveCommand<object> save;
		string pageTitle;
		string name;
		string minutes;
		string charge;
		ProductViewModel product;
		WorkUnitViewModel workUnit;


		public WorkUnitFormViewViewModel()
		{
			save = ReactiveCommand.Create(this.WhenAnyValue(x => x.Name, y => y.Minutes, z => z.Charge, (x, y, z) =>
			{
				double m, c = default(double);
				return !string.IsNullOrWhiteSpace(x)
				&& !string.IsNullOrWhiteSpace(y)
				&& !string.IsNullOrWhiteSpace(z)
				&& double.TryParse(y, out m)
				&& double.TryParse(z, out c);
			}));
			save
				.Subscribe(_ => SaveImpl());

		}

		public ReactiveCommand<object> Save => save;

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
			if (workUnit == null)
			{
				DataService.Current.Add(new WorkUnit
				{
					ProductId = product.Model.Id,
					Name = Name,
					Minutes = int.Parse(Minutes),
					Charge = double.Parse(Charge) / 60
				});
			}
			else
			{
				DataService.Current.Update(new WorkUnit
				{
					Id = workUnit.Model.Id,
					ProductId = product.Model.Id,
					Name = Name,
					Minutes = int.Parse(Minutes),
					Charge = double.Parse(Charge) / 60
				});
			}

			await NavigationService.Current.GoBack();
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				PageTitle = "Add Work";
				product = DataService.Current.GetProduct((Guid)parameter);
				Name = string.Empty;
				Minutes = string.Empty;
				Charge = string.Empty;
			}
			else
			{
				var p = (Tuple<Guid, Guid>)parameter;
				product = DataService.Current.GetProduct(p.Item1);
				workUnit = DataService.Current.GetWorkUnit(p.Item2);

				PageTitle = "Edit Work";
				Name = workUnit.Description;
				Minutes = workUnit.Minutes.ToString();
				Charge = workUnit.ChargePerHour.ToString();
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
