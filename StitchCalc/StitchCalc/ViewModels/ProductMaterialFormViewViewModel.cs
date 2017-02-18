using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.NavigationService;
using StitchCalc.Views;
using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Realms;

namespace StitchCalc.ViewModels
{
	public class ProductMaterialFormViewViewModel : ViewModelBase, INavigable
	{
		public ProductMaterialFormViewViewModel()
		{
            _materials = DBService.GetOrderedList<Material, string>(x => x.Name);

			_save = ReactiveCommand.Create(
				() => SaveImpl(),
				this.WhenAnyValue(x => x.SelectedMaterialIndex, y => y.Amount, (x, y) => x >= 0 && !string.IsNullOrWhiteSpace(y) && GetLengthFromAmountString(y) > 0));

			_navigateToMaterialFormView = ReactiveCommand.CreateFromTask(() => NavigationService.Current.NavigateTo<MaterialFormView>());
        }



        public ReactiveCommand Save => _save;

        public ReactiveCommand NavigateToMaterialFormView => _navigateToMaterialFormView;

        public IRealmCollection<Material> Materials => _materials;

        public string PageTitle
		{
			get { return _pageTitle; }
			set { this.RaiseAndSetIfChanged(ref _pageTitle, value); }
		}

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



        public Task OnNavigatedTo(object parameter, NavigationDirection direction)
        {
            if (direction == NavigationDirection.Forwards)
            {
                if (parameter is Guid)
                {
                    _product = DBService.GetSingle<Product>(x => x.Id == (Guid)parameter);

                    PageTitle = "Add Material";
                    SelectedMaterialIndex = 0;
                }
                else
                {
                    var param = (Tuple<Guid, Guid>)parameter;
                    _product = DBService.GetSingle<Product>(x => x.Id == param.Item1);
                    _productMaterial = _product.Materials.FirstOrDefault(x => x.Id == param.Item2);

                    PageTitle = "Edit Material";
                    SelectedMaterialIndex = _materials.ToList().IndexOf(_materials.FirstOrDefault(x => x.Id == _productMaterial.MaterialId));
                    Amount = _productMaterial.Length.ToString();
                }
            }

            return Task.CompletedTask;
        }

        public Task OnNavigatingFrom() => Task.CompletedTask;



        private async void SaveImpl()
		{
			var productMaterial = new ProductMaterial(_materials[_selectedMaterialIndex])
			{
				Length = GetLengthFromAmountString(Amount)
			};
            if (_productMaterial != null) productMaterial.Id = _productMaterial.Id;

            DBService.Write(realm => realm.Add(productMaterial, true));

			await NavigationService.Current.GoBack();
		}

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
        private readonly ReactiveCommand<Unit, Unit> _navigateToMaterialFormView;
        private string _pageTitle;
        private int _selectedMaterialIndex;
        private string _amount;
        private Product _product;
        private ProductMaterial _productMaterial;
    }
}
