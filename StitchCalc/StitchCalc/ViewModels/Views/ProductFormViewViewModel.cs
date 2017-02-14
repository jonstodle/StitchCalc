using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.DataServices;
using StitchCalc.Services.NavigationService;
using StitchCalc.ViewModels.Models;
using StitchCalc.Views;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels.Views
{
	public class ProductFormViewViewModel : ViewViewModelBase, INavigable
	{
		readonly ReactiveCommand<Unit, Unit> save;
		string pageTitle;
		ProductViewModel product;
		string name;

		public ProductFormViewViewModel()
		{
			save = ReactiveCommand.Create(
				() => AddMaterialsImpl(),
				this.WhenAnyValue(x => x.Name, x => !string.IsNullOrWhiteSpace(x)));
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

		private async void AddMaterialsImpl()
		{
			if (product == null)
			{
				var addedItem = DataService.Current.Add(new Product { Name = Name });
				await NavigationService.Current.NavigateToAndRemoveThis<ProductView>(Tuple.Create(addedItem.Id, 1));
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
				product = DataService.Current.GetProduct((Guid)parameter);

				PageTitle = "Edit Product";
				Name = product.Name;
			}
			else
			{
				PageTitle = "Add Product";
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
