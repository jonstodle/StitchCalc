using ReactiveUI;
using StitchCalc.Services.DataServices;
using StitchCalc.Services.NavigationService;
using StitchCalc.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels.Views
{
	public class ProductMaterialFormViewViewModel : ViewModelBase, INavigable
	{
		string pageTitle;
		ProductViewModel product;
		ProductMaterialViewModel productMaterial;

		public ProductMaterialFormViewViewModel()
		{

		}

		public string PageTitle
		{
			get { return pageTitle; }
			set { this.RaiseAndSetIfChanged(ref pageTitle, value); }
		}

		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				product = DataService.Current.GetProduct((Guid)parameter);

				PageTitle = "Add Material";
			}
			else
			{
				var param = (Tuple<Guid, Guid>)parameter;
				product = DataService.Current.GetProduct(param.Item1);
				productMaterial = DataService.Current.GetProductMaterial(param.Item2);

				PageTitle = "Edit Material";
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
