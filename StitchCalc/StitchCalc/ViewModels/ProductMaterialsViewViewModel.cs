﻿using ReactiveUI;
using StitchCalc.Services.NavigationService;
using StitchCalc.Views;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using StitchCalc.Models;

namespace StitchCalc.ViewModels
{
	public class ProductMaterialsViewViewModel : ViewModelBase, INavigable
	{
		readonly ReactiveCommand<Unit, Unit> navigateToMaterialFormView;
		readonly ReactiveCommand<Unit, Unit> edit;
		Product product;
		object selectedProductMaterial;

		public ProductMaterialsViewViewModel()
		{
			navigateToMaterialFormView = ReactiveCommand.CreateFromTask(() => NavigationService.Current.NavigateTo<ProductMaterialFormView>(product.Model.Id));

			edit = ReactiveCommand.CreateFromTask(x => NavigationService.Current.NavigateTo<ProductMaterialFormView>(Tuple.Create(product.Model.Id, (selectedProductMaterial as ProductMaterialViewModel).Model.Id)));
			edit
				.ThrownExceptions
				.Subscribe(ex => System.Diagnostics.Debug.WriteLine(ex.Message));
		}

		public ReactiveCommand NavigateToProductMaterialFormView => navigateToMaterialFormView;

		public ReactiveCommand Edit => edit;

		public Product Product
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