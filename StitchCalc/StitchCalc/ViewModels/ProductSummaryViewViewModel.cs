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
		public ProductSummaryViewViewModel()
		{
			_edit = ReactiveCommand.CreateFromTask(() => NavigationService.Current.NavigateTo<ProductFormView>(_model.Id));
		}

		public ReactiveCommand Edit => _edit;

		public Product Model
		{
			get { return _model; }
			set { this.RaiseAndSetIfChanged(ref _model, value); }
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				Model = DBService.GetSingle<Product>((Guid)parameter);
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;



        private readonly ReactiveCommand<Unit, Unit> _edit;
        private Product _model;
    }
}
