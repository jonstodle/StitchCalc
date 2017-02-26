using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Views;
using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Realms;
using StitchCalc.Services;
using System.Reactive.Linq;

namespace StitchCalc.ViewModels
{
	public class ProductMaterialFormViewModel : ViewModelBase
	{
		public ProductMaterialFormViewModel(Product product, ProductMaterial productMaterial = null)
		{
            _product = product;
            _productMaterial = productMaterial;
            Amount = _productMaterial?.Length.ToString();

            _materials = DBService.GetOrderedList<Material, string>(x => x.Name);

			_save = ReactiveCommand.CreateFromObservable(
				() => SaveImpl(),
				this.WhenAnyValue(x => x.SelectedMaterialIndex, y => y.Amount, (x, y) => x >= 0 && !string.IsNullOrWhiteSpace(y) && GetLengthFromAmountString(y) > 0));

			_addMaterial = ReactiveCommand.CreateFromObservable(() => Observable.FromAsync(() => NavigationService.NavigateTo(new MaterialFormView(new MaterialFormViewModel()))));

            if(_productMaterial != null) SelectedMaterialIndex = _materials.ToList().IndexOf(_materials.FirstOrDefault(x => x.Id == _productMaterial.MaterialId));
        }



        public ReactiveCommand Save => _save;

        public ReactiveCommand AddMaterial => _addMaterial;

        public IRealmCollection<Material> Materials => _materials;

        public string PageTitle => _productMaterial == null ? "Add Material" : "Edit Material";

        public int SelectedMaterialIndex
		{
			get { return _selectedMaterialIndex; }
			set { this.RaiseAndSetIfChanged(ref _selectedMaterialIndex, value); }
		}

		public string Amount
		{
			get { return _amount; }
			set { this.RaiseAndSetIfChanged(ref _amount, value); }
        }



        private IObservable<Unit> SaveImpl() => Observable.StartAsync(async () =>
        {
            var productMaterial = new ProductMaterial(_materials[_selectedMaterialIndex])
            {
                Product = _product,
                Length = GetLengthFromAmountString(Amount)
            };
            if (_productMaterial != null) productMaterial.Id = _productMaterial.Id;

            DBService.Write(realm => realm.Add(productMaterial, true));

            await NavigationService.GoBack();
        });

		private double GetLengthFromAmountString(string amountString)
		{
			double l, w;

			if (double.TryParse(amountString, out l)) { return l; }

			var amountParts = amountString.Split('x', 'X').Select(x => x.Trim()).ToList();

			if (amountParts.Count == 2 && double.TryParse(amountParts[0], out l) && double.TryParse(amountParts[1], out w))
			{
				return (l * w) / _materials[SelectedMaterialIndex >= 0 ? SelectedMaterialIndex : 0].Width;
			}

			return -1;
		}



        private readonly IRealmCollection<Material> _materials;
        private readonly ReactiveCommand<Unit, Unit> _save;
        private readonly ReactiveCommand<Unit, Unit> _addMaterial;
        private int _selectedMaterialIndex;
        private string _amount;
        private Product _product;
        private ProductMaterial _productMaterial;
    }
}
