using ReactiveUI;
using StitchCalc.Services.DataServices;
using StitchCalc.Services.NavigationService;
using StitchCalc.ViewModels.Models;
using StitchCalc.Views;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels.Views
{
	public class MaterialsViewViewModel : ViewModelBase, INavigable
	{
		readonly IReactiveDerivedList<MaterialViewModel> materials;
		readonly ReactiveCommand<object> navigateToMaterialFormView;
		readonly ReactiveCommand<object> edit;
		string searchTerm;
		object selectedMaterial;

		public MaterialsViewViewModel()
		{
			materials = DataService.Current.GetMaterials();

			navigateToMaterialFormView = ReactiveCommand.Create();
			navigateToMaterialFormView
				.Subscribe(_ => NavigationService.Current.NavigateTo<MaterialFormView>());

			edit = ReactiveCommand.Create();
			edit
				.Select(_ => selectedMaterial)
				.Cast<MaterialViewModel>()
				.Subscribe(item => NavigationService.Current.NavigateTo<MaterialFormView>(item.Model.Id));
		}

		public IReactiveDerivedList<MaterialViewModel> Materials => materials;

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

		public ReactiveCommand<object> NavigateToMaterialFormView => navigateToMaterialFormView;

		public ReactiveCommand<object> Edit => edit;



		public Task OnNavigatedTo(object parameter, NavigationDirection direction) => Task.CompletedTask;

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
