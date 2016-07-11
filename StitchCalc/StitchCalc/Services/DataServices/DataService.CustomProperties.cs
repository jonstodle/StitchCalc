using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StitchCalc.Services.DataServices
{
    public partial class DataService
    {
		readonly ReactiveList<CustomProperty> customProperties = new ReactiveList<CustomProperty>();
		public IReactiveDerivedList<CustomPropertyViewModel> GetCustomProperties() => customProperties.CreateDerivedCollection(x => new CustomPropertyViewModel(x));
		public IReactiveDerivedList<CustomPropertyViewModel> GetCustomPropertiesForProduct(Guid productId) => customProperties.CreateDerivedCollection(x => new CustomPropertyViewModel(x), x => x.ProductId == productId);
		public CustomPropertyViewModel GetCustomProperty(Guid customPropertyId)
		{
			var cp = customProperties.FirstOrDefault(x => x.Id == customPropertyId);
			return cp != default(CustomProperty) ? new CustomPropertyViewModel(cp) : default(CustomPropertyViewModel);
		}



		public CustomProperty Add(CustomProperty customProperty)
		{
			customProperty.Id = Guid.NewGuid();

			if (customProperty.ProductId == default(Guid)) { throw new ArgumentNullException(nameof(customProperty.ProductId)); }

			if (string.IsNullOrWhiteSpace(customProperty.Name)) { throw new ArgumentNullException(nameof(customProperty.Name)); }

			if (string.IsNullOrWhiteSpace(customProperty.Value)) { throw new ArgumentNullException(nameof(customProperty.Value)); }

			customProperties.Add(customProperty);

			return customProperty;
		}

		public CustomProperty Remove(CustomProperty customProperty)
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
				Add(customProperty);
			}

			return customProperty;
		}
	}
}
