using ReactiveUI;
using StitchCalc.Services.DataServices;
using StitchCalc.Services.NavigationService;
using StitchCalc.ViewModels.Models;
using StitchCalc.Views;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StitchCalc.ViewModels.Views
{
	public class MaterialsViewViewModel : ViewModelBase, INavigable
	{
		readonly IReactiveDerivedList<MaterialViewModel> materials;
		readonly ObservableAsPropertyHelper<IReactiveDerivedList<MaterialViewModel>> collectionView;
		readonly ReactiveCommand<object> navigateToMaterialFormView;
		readonly ReactiveCommand<object> edit;
		string searchTerm;

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

			navigateToMaterialFormView = ReactiveCommand.Create();
			navigateToMaterialFormView
				.Subscribe(async _ => await NavigationService.Current.NavigateTo<MaterialFormView>());

			edit = ReactiveCommand.Create();
			edit
				.Cast<EventPattern<ItemTappedEventArgs>>()
				.Select(x => x.EventArgs.Item)
				.Cast<MaterialViewModel>()
				.Subscribe(async item => await NavigationService.Current.NavigateTo<MaterialFormView>(item.Model.Id));
		}

		public IReactiveDerivedList<MaterialViewModel> Materials => materials;

		public IReactiveDerivedList<MaterialViewModel> CollectionView => collectionView.Value;

		public string SearchTerm
		{
			get { return searchTerm; }
			set { this.RaiseAndSetIfChanged(ref searchTerm, value); }
		}

		public ReactiveCommand<object> NavigateToMaterialFormView => navigateToMaterialFormView;

		public ReactiveCommand<object> Edit => edit;



		private IReactiveDerivedList<MaterialViewModel> CreateDerivedList(string searchString)
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
