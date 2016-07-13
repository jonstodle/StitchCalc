using ReactiveUI;
using StitchCalc.Services.DataServices;
using StitchCalc.Services.NavigationService;
using StitchCalc.ViewModels.Models;
using StitchCalc.Views;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels.Views
{
	public class ProductMaterialsViewViewModel : ViewModelBase, INavigable
	{
		readonly ReactiveCommand<object> navigateToMaterialFormView;
		readonly ReactiveCommand<object> edit;
		ProductViewModel product;
		object selectedProductMaterial;

		public ProductMaterialsViewViewModel()
		{
			navigateToMaterialFormView = ReactiveCommand.Create();
			navigateToMaterialFormView
				.Subscribe(_ => NavigationService.Current.NavigateTo<ProductMaterialFormView>(product.Model.Id));

			edit = ReactiveCommand.Create();
			edit
				.Select(_ => selectedProductMaterial)
				.Cast<ProductMaterialViewModel>()
				.Subscribe(item => NavigationService.Current.NavigateTo<ProductMaterialFormView>(Tuple.Create(product.Model.Id, item.Model.Id)));
		}

		public ReactiveCommand<object> NavigateToProductMaterialFormView => navigateToMaterialFormView;

		public ReactiveCommand<object> Edit => edit;

		public ProductViewModel Product
		{
			get { return product; }
			set { this.RaiseAndSetIfChanged(ref product, value); }
		}

		public object SelectedProductMaterial
		{
			get { return selectedProductMaterial; }
			set { this.RaiseAndSetIfChanged(ref selectedProductMaterial, value); }
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
