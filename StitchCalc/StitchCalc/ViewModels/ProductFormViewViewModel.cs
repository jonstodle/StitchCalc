using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Views;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels
{
	public class ProductFormViewViewModel : ViewModelBase
	{
		public ProductFormViewViewModel()
		{
			_save = ReactiveCommand.Create(
				() => SaveImpl(),
				this.WhenAnyValue(x => x.Name, x => !string.IsNullOrWhiteSpace(x)));
		}

		public ReactiveCommand Save => _save;

		public string PageTitle
		{
			get { return _pageTitle; }
			set { this.RaiseAndSetIfChanged(ref _pageTitle, value); }
		}

		public string Name
		{
			get { return _name; }
			set { this.RaiseAndSetIfChanged(ref _name, value); }
		}



        public Task OnNavigatedTo(object parameter, NavigationDirection direction)
        {
            if (parameter is Guid)
            {
                _product = DBService.GetSingle<Product>((Guid)parameter);

                PageTitle = "Edit Product";
                Name = _product.Name;
            }
            else
            {
                PageTitle = "Add Product";
            }

            return Task.CompletedTask;
        }

        public Task OnNavigatingFrom() => Task.CompletedTask;



		private async void SaveImpl()
		{
            var product = new Product { Name = _name };
            if (_product != null) product.Id = _product.Id;

            DBService.Write(realm => realm.Add(product, true));

            if (_product != null) await NavigationService.Current.GoBack();
            else await NavigationService.Current.NavigateToAndRemoveThis<ProductView>(Tuple.Create(product.Id, 1));
		}



        private readonly ReactiveCommand<Unit, Unit> _save;
        private string _pageTitle;
        private Product _product;
        private string _name;
    }
}
