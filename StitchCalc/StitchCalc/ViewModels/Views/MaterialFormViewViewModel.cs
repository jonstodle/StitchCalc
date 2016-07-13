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
	public class MaterialFormViewViewModel : ViewModelBase, INavigable
	{
		readonly ReactiveCommand<object> save;
		string pageTitle;
		string name;
		string width;
		string price;
		string description;
		MaterialViewModel material;

		public MaterialFormViewViewModel()
		{
			save = ReactiveCommand.Create(this.WhenAnyValue(a => a.Name, b => b.Width, c => c.Price, (a, b, c) =>
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

		public string Description
		{
			get { return description; }
			set { this.RaiseAndSetIfChanged(ref description, value); }
		}

		private async void SaveImpl()
		{
			var mtrl = new Material
			{
				Id = material?.Model.Id ?? default(Guid),
				Name = Name,
				Width = double.Parse(Width),
				Price = double.Parse(Price),
				Description = Description
			};
			
			if(material == null) { DataService.Current.Add(mtrl); }
			else { DataService.Current.Update(mtrl); }

			await NavigationService.Current.GoBack();
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				material = DataService.Current.GetMaterial((Guid)parameter);

				PageTitle = "Edit Material";
				Name = material.Model.Name;
				Width = material.Model.Width.ToString();
				Price = material.Model.Price.ToString();
				Description = material.Model.Description;
			}
			else
			{
				material = null;

				PageTitle = "Add Material";
				Name = string.Empty;
				Width = string.Empty;
				Price = string.Empty;
				Description = string.Empty;
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
