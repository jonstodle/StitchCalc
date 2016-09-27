using ReactiveUI;
using StitchCalc.Services.DataServices;
using StitchCalc.Services.NavigationService;
using StitchCalc.ViewModels.Models;
using StitchCalc.Views;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels.Views
{
	public class ProductSummaryViewViewModel : ViewModelBase, INavigable
	{
		readonly ReactiveCommand<Unit, Unit> edit;
		ProductViewModel model;

		public ProductSummaryViewViewModel()
		{
			edit = ReactiveCommand.CreateFromTask(() => NavigationService.Current.NavigateTo<ProductFormView>(model.Model.Id));
		}

		public ReactiveCommand Edit => edit;

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
