using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.DataServices;
using StitchCalc.Services.NavigationService;
using StitchCalc.ViewModels.Models;
using System;
using System.Collections.Generic;
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
		string length;
		ProductViewModel product;
		ProductMaterialViewModel productMaterial;

		public ProductMaterialFormViewViewModel()
		{
			materials = DataService.Current.GetMaterials();

			save = ReactiveCommand.Create(this.WhenAnyValue(x => x.SelectedMaterialIndex, y => y.Length, (x, y) =>
			{
				double l;
				return x >= 0 && double.TryParse(y, out l);
			}));
			save
				.Subscribe(_ => SaveImp());
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

		public string Length
		{
			get { return length; }
			set { this.RaiseAndSetIfChanged(ref length, value); }
		}

		private async void SaveImp()
		{
			var prdctmtrl = new ProductMaterial
			{
				Id = productMaterial?.Model.Id ?? default(Guid),
				ProductId = product.Model.Id,
				MaterialId = materials[selectedMaterialIndex].Model.Id,
				Length = double.Parse(Length)
			};

			if (productMaterial == null) { DataService.Current.Add(prdctmtrl); }
			else { DataService.Current.Update(prdctmtrl); }

			await NavigationService.Current.GoBack();
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				product = DataService.Current.GetProduct((Guid)parameter);

				PageTitle = "Add Material";
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
