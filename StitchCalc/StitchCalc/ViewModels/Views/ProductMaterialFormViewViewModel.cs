using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.DataServices;
using StitchCalc.Services.NavigationService;
using StitchCalc.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels.Views
{
	public class ProductMaterialFormViewViewModel : ViewModelBase, INavigable
	{
		readonly IReactiveDerivedList<MaterialViewModel> materials;
		readonly ReactiveCommand<object> save;
		string pageTitle;
		int selectedMaterialIndex;
		string amount;
		ProductViewModel product;
		ProductMaterialViewModel productMaterial;

		public ProductMaterialFormViewViewModel()
		{
			materials = DataService.Current.GetMaterials();

			save = ReactiveCommand.Create(this.WhenAnyValue(x => x.SelectedMaterialIndex, y => y.Amount, (x, y) => x >= 0 && !string.IsNullOrWhiteSpace(y) && GetLengthFromAmountString(y) > 0));
			save
				.Subscribe(_ => SaveImpl());
		}

		public string PageTitle
		{
			get { return pageTitle; }
			set { this.RaiseAndSetIfChanged(ref pageTitle, value); }
		}

		public ReactiveCommand<object> Save => save;

		public IReactiveDerivedList<MaterialViewModel> Materials => materials;

		public int SelectedMaterialIndex
		{
			get { return selectedMaterialIndex; }
			set { this.RaiseAndSetIfChanged(ref selectedMaterialIndex, value); }
		}

		public string Amount
		{
			get { return amount; }
			set { this.RaiseAndSetIfChanged(ref amount, value); }
		}

		private async void SaveImpl()
		{
			var prdctmtrl = new ProductMaterial
			{
				Id = productMaterial?.Model.Id ?? default(Guid),
				ProductId = product.Model.Id,
				MaterialId = materials[selectedMaterialIndex].Model.Id,
				Length = GetLengthFromAmountString(Amount)
			};

			if (productMaterial == null) { DataService.Current.Add(prdctmtrl); }
			else { DataService.Current.Update(prdctmtrl); }

			await NavigationService.Current.GoBack();
		}

		double GetLengthFromAmountString(string amountString)
		{
			double l, w;

			if (double.TryParse(amountString, out l)) { return l; }

			var amountParts = amountString.Split('x', 'X').Select(x => x.Trim()).ToList();

			if (amountParts.Count == 2 && double.TryParse(amountParts[0], out l) && double.TryParse(amountParts[1], out w))
			{
				return (l * w) / materials[SelectedMaterialIndex >= 0 ? SelectedMaterialIndex : 0].Width;
			}

			return -1;
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				product = DataService.Current.GetProduct((Guid)parameter);

				PageTitle = "Add Material";
				Amount = string.Empty;
			}
			else
			{
				var param = (Tuple<Guid, Guid>)parameter;
				product = DataService.Current.GetProduct(param.Item1);
				productMaterial = DataService.Current.GetProductMaterial(param.Item2);

				PageTitle = "Edit Material";
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
