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
	public class ProductSummaryViewViewModel : ViewModelBase, INavigable
	{
		readonly ReactiveCommand<object> edit;
		ProductViewModel model;

		public ProductSummaryViewViewModel()
		{
			edit = ReactiveCommand.Create();
			edit
				.Subscribe(_ => NavigationService.Current.NavigateTo<ProductFormView>(model.Model.Id));
		}

		public ReactiveCommand<object> Edit => edit;

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
