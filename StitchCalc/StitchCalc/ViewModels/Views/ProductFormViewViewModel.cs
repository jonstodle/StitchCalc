using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.DataServices;
using StitchCalc.Services.NavigationService;
using StitchCalc.ViewModels.Models;
using StitchCalc.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels.Views
{
	public class ProductFormViewViewModel : ViewModelBase, INavigable
	{
		readonly ReactiveCommand<object> addMaterials;
		string pageTitle;
		ProductViewModel product;
		string name;

		public ProductFormViewViewModel()
		{
			addMaterials = ReactiveCommand.Create(this.WhenAnyValue(x => x.Name, x => !string.IsNullOrWhiteSpace(x)));
			addMaterials
				.Subscribe(_ => AddMaterialsImpl());
		}

		public ReactiveCommand<object> AddMaterials => addMaterials;

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

		private async void AddMaterialsImpl()
		{
			if (product == null)
			{
				DataService.Current.Add(new Product { Name = Name });
				await NavigationService.Current.NavigateToAndRemoveThis<HomeView>();
			}
			else
			{
				DataService.Current.Update(new Product { Id = product.Model.Id, Name = Name });
				await NavigationService.Current.GoBack();
			}
		}

		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				PageTitle = "Edit Product";
				product = DataService.Current.GetProduct((Guid)parameter);
				Name = product.Name;
			}
			else { PageTitle = "Add Product"; }

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
