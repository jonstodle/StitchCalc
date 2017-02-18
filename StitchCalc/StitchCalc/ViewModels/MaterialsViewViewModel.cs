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
using System.Collections.Specialized;

namespace StitchCalc.ViewModels
{
	public class MaterialsViewViewModel : ViewModelBase, INavigable
	{
		public MaterialsViewViewModel()
		{
            _materials = DBService.GetOrderedList<Material, string>(x => x.Name);

			_navigateToMaterialFormView = ReactiveCommand.CreateFromTask(() => NavigationService.Current.NavigateTo<MaterialFormView>());

			_edit = ReactiveCommand.CreateFromTask(x => NavigationService.Current.NavigateTo<MaterialFormView>(_selectedMaterial.Id));

            _collectionView = Observable.Merge(
                    this.WhenAnyValue(x => x.SearchTerm),
                    _materials.Changed().Select(_ => ""))
                .Throttle(TimeSpan.FromMilliseconds(500), RxApp.TaskpoolScheduler)
                .Select(x => CreateFilteredList(x))
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, x => x.CollectionView, out _collectionView);
        }



        public ReactiveCommand NavigateToMaterialFormView => _navigateToMaterialFormView;

        public ReactiveCommand Edit => _edit;

        public IRealmCollection<Material> Materials => _materials;

		public IRealmCollection<Material> CollectionView => _collectionView.Value;

		public string SearchTerm
		{
			get { return _searchTerm; }
			set { this.RaiseAndSetIfChanged(ref _searchTerm, value); }
		}

		public Material SelectedMaterial
		{
			get { return _selectedMaterial; }
			set { this.RaiseAndSetIfChanged(ref _selectedMaterial, value); }
		}



        public Task OnNavigatedTo(object parameter, NavigationDirection direction) => Task.CompletedTask;

        public Task OnNavigatingFrom() => Task.CompletedTask;



        private IRealmCollection<Material> CreateFilteredList(string searchString)
		{
            if (searchString.HasValue()) return _materials;
            else return DBService.GetFilteredList<Material, string>(x => x.Name.ToLowerInvariant().Contains(searchString.ToLowerInvariant()), x => x.Name);
		}



        private readonly IRealmCollection<Material> _materials;
        private readonly ObservableAsPropertyHelper<IRealmCollection<Material>> _collectionView;
        private readonly ReactiveCommand<Unit, Unit> _navigateToMaterialFormView;
        private readonly ReactiveCommand<Unit, Unit> _edit;
        private string _searchTerm;
        private Material _selectedMaterial;
    }
}
