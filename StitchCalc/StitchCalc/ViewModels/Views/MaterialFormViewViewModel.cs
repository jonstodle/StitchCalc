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
		string amount;
		string price;
		string description;
		ProductViewModel product;
		MaterialViewModel material;

		public MaterialFormViewViewModel()
		{
			save = ReactiveCommand.Create(this.WhenAnyValue(a => a.Name, b => b.Amount, c => c.Price, (a, b, c) =>
			{
				double amnt, prc = default(double);
				return !string.IsNullOrWhiteSpace(a)
				&& !string.IsNullOrWhiteSpace(b)
				&& !string.IsNullOrWhiteSpace(b)
				&& double.TryParse(b, out amnt)
				&& double.TryParse(c, out prc);
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

		public string Amount
		{
			get { return amount; }
			set { this.RaiseAndSetIfChanged(ref amount, value); }
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
				ProductId = product.Model.Id,
				Name = Name,
				Amount = double.Parse(Amount),
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
				product = DataService.Current.GetProduct((Guid)parameter);

				PageTitle = "Add Material";
				Name = string.Empty;
				Amount = string.Empty;
				Price = string.Empty;
				Description = string.Empty;
			}
			else
			{
				var param = (Tuple<Guid, Guid>)parameter;
				product = DataService.Current.GetProduct(param.Item1);
				material = DataService.Current.GetMaterial(param.Item2);

				PageTitle = "Edit Material";
				Name = string.Empty;
				Amount = string.Empty;
				Price = string.Empty;
				Description = string.Empty;
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
