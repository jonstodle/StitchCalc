using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System.Reactive.Linq;

using Xamarin.Forms;

namespace StitchCalc.Views
{
	public partial class MaterialFormView : ContentPage, IViewFor<MaterialFormViewViewModel>
	{
		public MaterialFormView ()
		{
			InitializeComponent ();

			this.OneWayBind(ViewModel, vm => vm.PageTitle, v => v.Title);
			this.Bind(ViewModel, vm => vm.Name, v => v.NameEntry.Text);
			this.Bind(ViewModel, vm => vm.Width, v => v.WidthEntry.Text);
			this.Bind(ViewModel, vm => vm.Price, v => v.PriceEntry.Text);
			this.BindCommand(ViewModel, vm => vm.Save, v => v.SaveToolbarItem);
			this.OneWayBind(ViewModel, vm => vm.CanAddProperty, v => v.CustomPropertiesStackLayout.IsVisible);
			this.OneWayBind(ViewModel, vm => vm.ToggleAddGridText, v => v.CustomPropertyToggleAddGridButton.Text);
			this.BindCommand(ViewModel, vm => vm.ToggleShowAddGrid, v => v.CustomPropertyToggleAddGridButton);
			this.OneWayBind(ViewModel, vm => vm.ShowAddGrid, v => v.CustomPropertyAddGrid.IsVisible);
			this.Bind(ViewModel, vm => vm.CustomPropertyName, v => v.CustomPropertyNameEntry.Text);
			this.Bind(ViewModel, vm => vm.CustomPropertyValue, v => v.CustomPropertyValueEntry.Text);
			this.BindCommand(ViewModel, vm => vm.AddProperty, v => v.CustomPropertyAddButton);
			this.OneWayBind(ViewModel, vm => vm.CustomProperties, v => v.CustomPropertiesListView.ItemsSource);

			Observable
				.Merge(
				Observable.FromEventPattern(NameEntry, nameof(Entry.Completed)),
				Observable.FromEventPattern(WidthEntry, nameof(Entry.Completed)),
				Observable.FromEventPattern(PriceEntry, nameof(Entry.Completed)))
				.InvokeCommand(ViewModel, x => x.Save);

			Observable
				.Merge(
				Observable.FromEventPattern(CustomPropertyNameEntry, nameof(Entry.Completed)),
				Observable.FromEventPattern(CustomPropertyValueEntry, nameof(Entry.Completed)))
				.InvokeCommand(ViewModel, x => x.AddProperty);
		}

		public MaterialFormViewViewModel ViewModel
		{
			get { return (MaterialFormViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(MaterialFormViewViewModel), typeof(MaterialFormView), new MaterialFormViewViewModel());

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (MaterialFormViewViewModel)value; }
		}
	}
}
