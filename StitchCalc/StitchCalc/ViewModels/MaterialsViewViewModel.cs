using ReactiveUI;
using StitchCalc.Services.NavigationService;
using StitchCalc.Views;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Realms;
using StitchCalc.Models;

namespace StitchCalc.ViewModels
{
	public class MaterialsViewViewModel : ViewModelBase, INavigable
	{
		readonly IRealmCollection<Material> materials;
		readonly ObservableAsPropertyHelper<IRealmCollection<Material>> collectionView;
		readonly ReactiveCommand<Unit, Unit> navigateToMaterialFormView;
		readonly ReactiveCommand<Unit, Unit> edit;
		string searchTerm;
		object selectedMaterial;

		public MaterialsViewViewModel()
		{
			materials = DataService.Current.GetMaterials();

			this
				.WhenAnyValue(x => x.SearchTerm)
				.Merge(materials.Changed.Select(_ => ""))
				.Throttle(TimeSpan.FromMilliseconds(500), RxApp.TaskpoolScheduler)
				.Select(x => CreateDerivedList(x))
				.ObserveOn(RxApp.MainThreadScheduler)
				.ToProperty(this, x => x.CollectionView, out collectionView);

			navigateToMaterialFormView = ReactiveCommand.CreateFromTask(() => NavigationService.Current.NavigateTo<MaterialFormView>());

			edit = ReactiveCommand.CreateFromTask(x => NavigationService.Current.NavigateTo<MaterialFormView>((selectedMaterial as MaterialViewModel).Model.Id));
		}

		public IRealmCollection<Material> Materials => materials;

		public IRealmCollection<Material> CollectionView => collectionView.Value;

		public string SearchTerm
		{
			get { return searchTerm; }
			set { this.RaiseAndSetIfChanged(ref searchTerm, value); }
		}

		public object SelectedMaterial
		{
			get { return selectedMaterial; }
			set { this.RaiseAndSetIfChanged(ref selectedMaterial, value); }
		}

		public ReactiveCommand NavigateToMaterialFormView => navigateToMaterialFormView;

		public ReactiveCommand Edit => edit;



		private IRealmCollection<Material> CreateDerivedList(string searchString)
		{
			var orderFunc = new Func<MaterialViewModel, MaterialViewModel, int>((item1, item2) => item1.Name.CompareTo(item2.Name));

			if (string.IsNullOrWhiteSpace(searchString))
			{
				return Materials.CreateDerivedCollection(x => x, orderer: orderFunc);
			}
			else
			{
				return Materials.CreateDerivedCollection(x => x, x => x.Name.ToLowerInvariant().Contains(searchString.ToLowerInvariant()), orderFunc);
			}
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction) => Task.CompletedTask;

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
