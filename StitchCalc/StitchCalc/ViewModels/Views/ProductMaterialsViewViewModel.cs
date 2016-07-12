using ReactiveUI;
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
	public class ProductMaterialsViewViewModel : ViewModelBase, INavigable
	{
		readonly ReactiveCommand<object> navigateToMaterialFormView;
		ProductViewModel product;

		public ProductMaterialsViewViewModel()
		{
			navigateToMaterialFormView = ReactiveCommand.Create();
			navigateToMaterialFormView
				.Subscribe(_ => NavigationService.Current.NavigateTo<MaterialFormView>(product.Model.Id));
		}

		public ReactiveCommand<object> NavigateToMaterialFormView => navigateToMaterialFormView;

		public ProductViewModel Product
		{
			get { return product; }
			set { this.RaiseAndSetIfChanged(ref product, value); }
		}

		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				Product = DataService.Current.GetProduct((Guid)parameter);
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
