using ReactiveUI;
using StitchCalc.Services.NavigationService;
using StitchCalc.Views;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using StitchCalc.Models;
using Realms;
using System.Collections.Specialized;
using System.Linq;

namespace StitchCalc.ViewModels
{
	public class ProductMaterialsViewViewModel : ViewModelBase, INavigable
	{
		public ProductMaterialsViewViewModel()
		{
			_navigateToMaterialFormView = ReactiveCommand.CreateFromTask(() => NavigationService.Current.NavigateTo<ProductMaterialFormView>(_product.Id));

			_edit = ReactiveCommand.CreateFromTask(x => NavigationService.Current.NavigateTo<ProductMaterialFormView>(Tuple.Create(_product.Id, _selectedProductMaterial.Id)));
			_edit
				.ThrownExceptions
				.Subscribe(ex => System.Diagnostics.Debug.WriteLine(ex.Message));

            _materialsPrice = this.WhenAnyValue(x => x.Product)
                .WhereNotNull()
                .SelectMany(x => x.Materials.AsRealmCollection().Changed().Select(_ => x.Materials.ToList()))
                .Select(x => x.Sum(y => y.Price))
                .ToProperty(this, x => x.MaterialsPrice);
		}



		public ReactiveCommand NavigateToProductMaterialFormView => _navigateToMaterialFormView;

		public ReactiveCommand Edit => _edit;

        public double MaterialsPrice => _materialsPrice.Value;

		public Product Product
		{
			get { return _product; }
			set { this.RaiseAndSetIfChanged(ref _product, value); }
		}

		public ProductMaterial SelectedProductMaterial
		{
			get { return _selectedProductMaterial; }
			set { this.RaiseAndSetIfChanged(ref _selectedProductMaterial, value); }
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				Product = DBService.GetSingle<Product>((Guid)parameter);
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;



        private readonly ReactiveCommand<Unit, Unit> _navigateToMaterialFormView;
        private readonly ReactiveCommand<Unit, Unit> _edit;
        private readonly ObservableAsPropertyHelper<double> _materialsPrice;
        private Product _product;
        private ProductMaterial _selectedProductMaterial;
    }
}
