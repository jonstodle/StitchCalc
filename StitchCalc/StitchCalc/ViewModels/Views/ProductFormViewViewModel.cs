using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.DataServices;
using StitchCalc.Services.NavigationService;
using StitchCalc.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels.Views
{
	public class ProductFormViewViewModel : ViewModelBase, INavigable
	{
		readonly ReactiveCommand<object> addMaterials;
		string name;

		public ProductFormViewViewModel()
		{
			addMaterials = ReactiveCommand.Create(this.WhenAnyValue(x => x.Name, x => !string.IsNullOrWhiteSpace(x)));
			addMaterials
				.Subscribe(_ => AddMaterialsImpl());
		}

		public ReactiveCommand<object> AddMaterials => addMaterials;

		public string Name
		{
			get { return name; }
			set { this.RaiseAndSetIfChanged(ref name, value); }
		}

		private async void AddMaterialsImpl()
		{
			DataService.Current.Add(new Product { Name = Name });
			await NavigationService.Current.NavigateToAndRemoveThis<HomeView>();
		}

		public Task OnNavigatedTo(object parameter, NavigationDirection direction) => Task.CompletedTask;

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
