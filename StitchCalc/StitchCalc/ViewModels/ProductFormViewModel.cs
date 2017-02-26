using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services;
using StitchCalc.Views;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels
{
	public class ProductFormViewModel : ViewModelBase
	{
		public ProductFormViewModel(Product product = null)
		{
            _product = product;
            Name = _product?.Name;

            _save = ReactiveCommand.CreateFromObservable(
				() => SaveImpl(),
				this.WhenAnyValue(x => x.Name, x => !string.IsNullOrWhiteSpace(x)));
		}

		public ReactiveCommand Save => _save;

		public string PageTitle => _product == null ? "Add Product" : "Edit Product";


        public string Name
		{
			get { return _name; }
			set { this.RaiseAndSetIfChanged(ref _name, value); }
		}



        private IObservable<Unit> SaveImpl() => Observable.StartAsync(async () =>
        {
            var product = new Product { Name = _name };
            if (_product != null) product.Id = _product.Id;

            DBService.Write(realm => realm.Add(product, true));

            if (_product != null) await NavigationService.GoBack();
            else await NavigationService.NavigateToAndRemoveThis(new ProductView(new ProductViewModel(_product)));
        });



        private readonly ReactiveCommand<Unit, Unit> _save;
        private Product _product;
        private string _name;
    }
}
