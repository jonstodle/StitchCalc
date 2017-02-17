using ReactiveUI;
using StitchCalc.Services.NavigationService;
using StitchCalc.Views;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using StitchCalc.Models;

namespace StitchCalc.ViewModels
{
	public class ProductSummaryViewViewModel : ViewModelBase, INavigable
	{
		readonly ReactiveCommand<Unit, Unit> edit;
		Product model;

		public ProductSummaryViewViewModel()
		{
			edit = ReactiveCommand.CreateFromTask(() => NavigationService.Current.NavigateTo<ProductFormView>(model.Model.Id));
		}

		public ReactiveCommand Edit => edit;

		public Product Model
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
