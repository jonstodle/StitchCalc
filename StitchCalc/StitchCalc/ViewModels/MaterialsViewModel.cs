using ReactiveUI;
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
	public class MaterialsViewModel : ViewModelBase
	{
		public MaterialsViewModel()
		{
            _materials = DBService.GetOrderedList<Material, string>(x => x.Name);

			_navigateToMaterialFormView = ReactiveCommand.CreateFromTask(() => NavigationService.Current.NavigateTo<MaterialFormView>());

			_edit = ReactiveCommand.CreateFromTask(x => NavigationService.Current.NavigateTo<MaterialFormView>(_selectedMaterial.Id));

			_materialsView = Observable.Merge(
                    this.WhenAnyValue(x => x.SearchTerm),
                    _materials.CollectionChanges().Select(_ => ""))
                .Throttle(TimeSpan.FromMilliseconds(500), RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
				.Select(x => CreateFilteredList(x))
                .ToProperty(this, x => x.MaterialsView, out _materialsView);
        }



        public ReactiveCommand NavigateToMaterialFormView => _navigateToMaterialFormView;

        public ReactiveCommand Edit => _edit;

        public IRealmCollection<Material> Materials => _materials;

		public IReactiveDerivedList<MaterialViewModel> MaterialsView => _materialsView.Value;

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



		private IReactiveDerivedList<MaterialViewModel> CreateFilteredList(string searchString)
		{
			if (!searchString.HasValue()) return _materials.CreateDerivedCollection(x => new MaterialViewModel(x));
			else return DBService.GetFilteredList<Material, string>(x => x.Name.Contains(searchString), x => x.Name).CreateDerivedCollection(x => new MaterialViewModel(x));
		}



        private readonly ReactiveCommand<Unit, Unit> _navigateToMaterialFormView;
        private readonly ReactiveCommand<Unit, Unit> _edit;
        private readonly IRealmCollection<Material> _materials;
		private readonly ObservableAsPropertyHelper<IReactiveDerivedList<MaterialViewModel>> _materialsView;
        private string _searchTerm;
        private Material _selectedMaterial;
    }
}
