using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.DataServices;
using StitchCalc.Services.NavigationService;
using StitchCalc.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels.Views
{
	public class MaterialFormViewViewModel : ViewModelBase, INavigable
	{
		readonly ReactiveCommand<object> save;
		readonly ReactiveCommand<object> addProperty;
		readonly ReactiveCommand<object> toggleShowAddGrid;
		readonly ObservableAsPropertyHelper<bool> canAddProperty;
		readonly ObservableAsPropertyHelper<string> toggleAddGridText;
		IReactiveDerivedList<CustomPropertyViewModel> customProperties;
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
			customProperties = DataService.Current.GetCustomPropertiesForParent(new Guid());

			save = ReactiveCommand.Create(this.WhenAnyValue(a => a.Name, b => b.Width, c => c.Price, (a, b, c) =>
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
				.Subscribe(_ => SaveImpl());
			save
				.CanExecuteObservable
				.ToProperty(this, x => x.CanAddProperty, out canAddProperty);

			addProperty = ReactiveCommand.Create(this.WhenAnyValue(x => x.CustomPropertyName, y=> y.CustomPropertyValue, z => z.CanAddProperty, (x,y,z)=> !string.IsNullOrWhiteSpace(x) && !string.IsNullOrWhiteSpace(y) && z));
			addProperty
				.Subscribe(_ => AddPropertyImpl());

			toggleShowAddGrid = ReactiveCommand.Create();
			toggleShowAddGrid
				.Select(_ => !ShowAddGrid)
				.Subscribe(x => ShowAddGrid = x);

			this
				.WhenAnyValue(x => x.ShowAddGrid)
				.Select(x => x ? "\u25B3" : "\u25BD")
				.ToProperty(this, x => x.ToggleAddGridText, out toggleAddGridText);
		}

		public ReactiveCommand<object> Save => save;

		public ReactiveCommand<object> AddProperty => addProperty;

		public ReactiveCommand<object> ToggleShowAddGrid => toggleShowAddGrid;

		public bool CanAddProperty => canAddProperty.Value;

		public string ToggleAddGridText => toggleAddGridText.Value;

		public IReactiveDerivedList<CustomPropertyViewModel> CustomProperties
		{
			get { return customProperties; }
			set { this.RaiseAndSetIfChanged(ref customProperties, value); }
		}

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



		async void SaveImpl()
		{
			SaveMaterial();

			await NavigationService.Current.GoBack();
		}

		void AddPropertyImpl()
		{
			if (material == null)
			{
				material = DataService.Current.GetMaterial(SaveMaterial().Id);
				CustomProperties = DataService.Current.GetCustomPropertiesForParent(material.Model.Id);
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

			if (material == null) { return DataService.Current.Add(mtrl); }
			else { return DataService.Current.Update(mtrl); }
		}



		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				material = DataService.Current.GetMaterial((Guid)parameter);

				CustomProperties = DataService.Current.GetCustomPropertiesForParent(material.Model.Id);

				PageTitle = "Edit Material";
				Name = material.Model.Name;
				Width = material.Model.Width.ToString();
				Price = material.Model.Price.ToString();
			}
			else
			{
				material = null;

				PageTitle = "Add Material";
				Name = string.Empty;
				Width = string.Empty;
				Price = string.Empty;

				ShowAddGrid = true;
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
