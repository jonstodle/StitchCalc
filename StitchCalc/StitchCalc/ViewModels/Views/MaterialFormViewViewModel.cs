using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.DataServices;
using StitchCalc.Services.NavigationService;
using StitchCalc.ViewModels.Models;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels.Views
{
	public class MaterialFormViewViewModel : ViewModelBase, INavigable
	{
		readonly ReactiveCommand<Unit, Unit> save;
		readonly ReactiveCommand<Unit, Unit> addProperty;
		readonly ReactiveCommand<Unit, Unit> toggleShowAddGrid;
		readonly ObservableAsPropertyHelper<bool> canAddProperty;
		readonly ObservableAsPropertyHelper<IReactiveDerivedList<CustomPropertyViewModel>> customProperties;
		string pageTitle;
		string name;
		string width;
		string price;
		bool showAddGrid;
		string customPropertyName;
		string customPropertyValue;
		MaterialViewModel material;

		public MaterialFormViewViewModel()
		{
			save = ReactiveCommand.CreateFromTask(
				() => SaveImpl(),
				this.WhenAnyValue(a => a.Name, b => b.Width, c => c.Price, (a, b, c) =>
				{
					double amnt, prc = default(double);
					return !string.IsNullOrWhiteSpace(a)
					&& !string.IsNullOrWhiteSpace(b)
					&& !string.IsNullOrWhiteSpace(b)
					&& double.TryParse(b, out amnt)
					&& double.TryParse(c, out prc)
					&& amnt > 0
					&& prc > 0;
				}));
			save
				.CanExecute
				.ToProperty(this, x => x.CanAddProperty, out canAddProperty);

			addProperty = ReactiveCommand.Create(
				() => AddPropertyImpl(),
				this.WhenAnyValue(x => x.CustomPropertyName, y=> y.CustomPropertyValue, z => z.CanAddProperty, (x,y,z)=> !string.IsNullOrWhiteSpace(x) && !string.IsNullOrWhiteSpace(y) && z));

			toggleShowAddGrid = ReactiveCommand.Create(() => { ShowAddGrid = !ShowAddGrid; });

			customProperties = Observable.Merge(
				this.WhenAnyValue(x => x.Material).WhereNotNull().Select(x => DataService.Current.GetCustomPropertiesForParent(x.Model.Id)),
				DataService.Current.GetCustomProperties().Changed.Select(x => DataService.Current.GetCustomPropertiesForParent(Material.Model.Id)))
				.ToProperty(this, x => x.CustomProperties, DataService.Current.GetCustomPropertiesForParent(new Guid()));
		}

		public ReactiveCommand Save => save;

		public ReactiveCommand AddProperty => addProperty;

		public ReactiveCommand ToggleShowAddGrid => toggleShowAddGrid;

		public bool CanAddProperty => canAddProperty.Value;

		public IReactiveDerivedList<CustomPropertyViewModel> CustomProperties => customProperties.Value;

		public string PageTitle
		{
			get { return pageTitle; }
			set { this.RaiseAndSetIfChanged(ref pageTitle, value); }
		}

		public string Name
		{
			get { return name; }
			set { this.RaiseAndSetIfChanged(ref name, value); }
		}

		public string Width
		{
			get { return width; }
			set { this.RaiseAndSetIfChanged(ref width, value); }
		}

		public string Price
		{
			get { return price; }
			set { this.RaiseAndSetIfChanged(ref price, value); }
		}

		public bool ShowAddGrid
		{
			get { return showAddGrid; }
			set { this.RaiseAndSetIfChanged(ref showAddGrid, value); }
		}

		public string CustomPropertyName
		{
			get { return customPropertyName; }
			set { this.RaiseAndSetIfChanged(ref customPropertyName, value); }
		}

		public string CustomPropertyValue
		{
			get { return customPropertyValue; }
			set { this.RaiseAndSetIfChanged(ref customPropertyValue, value); }
		}

		public MaterialViewModel Material
		{
			get { return material; }
			set { this.RaiseAndSetIfChanged(ref material, value); }
		}



		async Task SaveImpl()
		{
			SaveMaterial();

			await NavigationService.Current.GoBack();
		}

		void AddPropertyImpl()
		{
			if (Material == null)
			{
				Material = DataService.Current.GetMaterial(SaveMaterial().Id);
			}

			var cp = new CustomProperty
			{
				ParentId = material.Model.Id,
				Name = CustomPropertyName,
				Value = CustomPropertyValue
			};

			DataService.Current.Add(cp);

			CustomPropertyName = string.Empty;
			CustomPropertyValue = string.Empty;
		}

		Material SaveMaterial()
		{
			var mtrl = new Material
			{
				Id = material?.Model.Id ?? default(Guid),
				Name = Name,
				Width = double.Parse(Width),
				Price = double.Parse(Price)
			};

			if (Material == null) { return DataService.Current.Add(mtrl); }
			else { return DataService.Current.Update(mtrl); }
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				Material = DataService.Current.GetMaterial((Guid)parameter);

				PageTitle = "Edit Material";
				Name = material.Model.Name;
				Width = material.Model.Width.ToString();
				Price = material.Model.Price.ToString();
			}
			else
			{
				PageTitle = "Add Material";
				ShowAddGrid = true;
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
