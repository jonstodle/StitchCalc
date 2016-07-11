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
	public class ProductWorkUnitsViewViewModel : ViewModelBase, INavigable
	{
		ProductViewModel model;

		public ProductWorkUnitsViewViewModel()
		{

		}

		public ProductViewModel Model
		{
			get { return model; }
			set { this.RaiseAndSetIfChanged(ref model, value); }
		}

		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				Model = DataService.Current.GetProduct((Guid)parameter);
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
