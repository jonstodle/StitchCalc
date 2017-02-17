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
		readonly IRealmCollection<Material> materials;
		readonly ReactiveCommand<Unit, Unit> save;
		readonly ReactiveCommand<Unit, Unit> navigateToMaterialFormView;
		string pageTitle;
		int selectedMaterialIndex;
		string amount;
		Product product;
		ProductMaterial productMaterial;

		public ProductMaterialFormViewViewModel()
		{
			materials = DataService.Current.GetMaterials();

			save = ReactiveCommand.Create(
				() => SaveImpl(),
				this.WhenAnyValue(x => x.SelectedMaterialIndex, y => y.Amount, (x, y) => x >= 0 && !string.IsNullOrWhiteSpace(y) && GetLengthFromAmountString(y) > 0));

			navigateToMaterialFormView = ReactiveCommand.CreateFromTask(() => NavigationService.Current.NavigateTo<MaterialFormView>());
		}

		public string PageTitle
		{
			get { return pageTitle; }
			set { this.RaiseAndSetIfChanged(ref pageTitle, value); }
		}

		public ReactiveCommand Save => save;

		public ReactiveCommand NavigateToMaterialFormView => navigateToMaterialFormView;

		public IRealmCollection<Material> Materials => materials;

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
			if (direction == NavigationDirection.Forwards)
			{
				if (parameter is Guid)
				{
					product = DataService.Current.GetProduct((Guid)parameter);

					PageTitle = "Add Material";
					SelectedMaterialIndex = materials.Count > 0 ? 0 : default(int);
				}
				else
				{
					var param = (Tuple<Guid, Guid>)parameter;
					product = DataService.Current.GetProduct(param.Item1);
					productMaterial = DataService.Current.GetProductMaterial(param.Item2);

					PageTitle = "Edit Material";
					for (int i = 0; i < materials.Count; i++)
					{
						if (materials[i].Model.Id == productMaterial.Model.MaterialId)
						{
							SelectedMaterialIndex = i;
							break;
						}
					}
					Amount = productMaterial.Length.ToString();
				} 
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
