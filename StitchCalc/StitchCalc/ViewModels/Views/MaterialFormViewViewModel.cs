using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.DataServices;
using StitchCalc.Services.NavigationService;
using StitchCalc.ViewModels.Models;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels.Views
{
	public class MaterialFormViewViewModel : ViewModelBase, INavigable
	{
		readonly ReactiveCommand<Unit, Unit> save;
		string pageTitle;
		string name;
		string width;
		string price;
		string notes;
		MaterialViewModel material;

		public MaterialFormViewViewModel()
		{
			save = ReactiveCommand.CreateFromTask(
				() => SaveImpl(),
				this.WhenAnyValue(a => a.Name, b => b.Width, c => c.Price, (a, b, c) =>
				{
					double amnt, prc = default(double);
					return !string.IsNullOrWhiteSpace(a)
					&& !string.IsNullOrWhiteSpace(b)
					&& !string.IsNullOrWhiteSpace(b)
					&& double.TryParse(b, out amnt)
					&& double.TryParse(c, out prc)
					&& amnt > 0
					&& prc > 0;
				}));
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

		public string Width
		{
			get { return width; }
			set { this.RaiseAndSetIfChanged(ref width, value); }
		}

		public string Price
		{
			get { return price; }
			set { this.RaiseAndSetIfChanged(ref price, value); }
		}

		public string Notes
		{
			get { return notes; }
			set { this.RaiseAndSetIfChanged(ref notes, value); }
		}

		public MaterialViewModel Material
		{
			get { return material; }
			set { this.RaiseAndSetIfChanged(ref material, value); }
		}



		async Task SaveImpl()
		{
			SaveMaterial();

			await NavigationService.Current.GoBack();
		}

		Material SaveMaterial()
		{
			var mtrl = new Material
			{
				Id = material?.Model.Id ?? default(Guid),
				Name = Name,
				Width = double.Parse(Width),
				Price = double.Parse(Price),
				Notes = notes
			};

			if (Material == null) { return DataService.Current.Add(mtrl); }
			else { return DataService.Current.Update(mtrl); }
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				Material = DataService.Current.GetMaterial((Guid)parameter);

				PageTitle = "Edit Material";
				Name = material.Model.Name;
				Width = material.Model.Width.ToString();
				Price = material.Model.Price.ToString();
		Notes = material.Model.Notes;
			}
			else
			{
				PageTitle = "Add Material";
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
