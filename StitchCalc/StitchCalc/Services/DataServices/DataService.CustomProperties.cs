using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.ViewModels.Models;
using System;
using System.Linq;

namespace StitchCalc.Services.DataServices
{
	public partial class DataService
    {
		readonly ReactiveList<CustomProperty> customProperties = new ReactiveList<CustomProperty>();
		public IReactiveDerivedList<CustomPropertyViewModel> GetCustomProperties() => customProperties.CreateDerivedCollection(x => new CustomPropertyViewModel(x));
		public IReactiveDerivedList<CustomPropertyViewModel> GetCustomPropertiesForParent(Guid parentId) => customProperties.CreateDerivedCollection(x => new CustomPropertyViewModel(x), x => x.ParentId == parentId);
		public CustomPropertyViewModel GetCustomProperty(Guid customPropertyId)
		{
			var cp = customProperties.FirstOrDefault(x => x.Id == customPropertyId);
			return cp != default(CustomProperty) ? new CustomPropertyViewModel(cp) : default(CustomPropertyViewModel);
		}



		public CustomProperty Add(CustomProperty customProperty)
		{
			customProperty.Id = Guid.NewGuid();

			if (customProperty.ParentId == default(Guid)) { throw new ArgumentNullException(nameof(customProperty.ParentId)); }

			if (string.IsNullOrWhiteSpace(customProperty.Name)) { throw new ArgumentNullException(nameof(customProperty.Name)); }

			if (string.IsNullOrWhiteSpace(customProperty.Value)) { throw new ArgumentNullException(nameof(customProperty.Value)); }

			customProperties.Add(customProperty);

			return customProperty;
		}

		public bool Remove(CustomProperty customProperty)
		{
			var cp = customProperties.FirstOrDefault(x => x.Id == customProperty.Id);

			if (cp == default(CustomProperty)) { throw new ArgumentException("CustomProperty id not found"); }

			return customProperties.Remove(cp);
		}

		public CustomProperty Update(CustomProperty customProperty)
		{
			var cp = customProperties.FirstOrDefault(x => x.Id == customProperty.Id);

			if (cp == default(CustomProperty)) { throw new ArgumentException("CustomProperty id not found"); }

			using (customProperties.SuppressChangeNotifications())
			{
				Remove(cp);
				Add(customProperty).Id = cp.Id;
			}

			return customProperty;
		}
	}
}
