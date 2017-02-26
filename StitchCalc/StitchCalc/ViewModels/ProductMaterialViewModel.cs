using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services;
using StitchCalc.Views;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace StitchCalc.ViewModels
{
    public class ProductMaterialViewModel : ViewModelBase
    {
        public ProductMaterialViewModel(ProductMaterial productMaterial)
        {
            _productMaterial = productMaterial;

            _edit = ReactiveCommand.CreateFromObservable(() => Observable.FromAsync(() => NavigationService.NavigateTo(new ProductMaterialFormView(new ProductMaterialFormViewModel(_productMaterial.Product, _productMaterial)))));
        }



        public ReactiveCommand Edit => _edit;

        public ProductMaterial ProductMaterial => _productMaterial;



        private readonly ReactiveCommand<Unit, Unit> _edit;
        private readonly ProductMaterial _productMaterial;
    }
}
