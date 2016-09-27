using ReactiveUI;
using StitchCalc.ViewModels.Models;
using StitchCalc.ViewModels.Views;
using System.Reactive.Linq;
using System;

using Xamarin.Forms;
using System.Reactive;

namespace StitchCalc.Views
{
	public partial class MaterialFormView : ContentPage, IViewFor<MaterialFormViewViewModel>
	{
		public MaterialFormView()
		{
			InitializeComponent();

			ViewModel = new MaterialFormViewViewModel();

			this.OneWayBind(ViewModel, vm => vm.PageTitle, v => v.Title);
			this.Bind(ViewModel, vm => vm.Name, v => v.NameEntry.Text);
			this.Bind(ViewModel, vm => vm.Width, v => v.WidthEntry.Text);
			this.Bind(ViewModel, vm => vm.Price, v => v.PriceEntry.Text);
			this.OneWayBind(ViewModel, vm => vm.CanAddProperty, v => v.CustomPropertiesStackLayout.IsVisible);
			this.OneWayBind(ViewModel, vm => vm.ShowAddGrid, v => v.CustomPropertyToggleAddGridButton.Text, x => x ? "\u25B3" : "\u25BD");
			this.OneWayBind(ViewModel, vm => vm.ShowAddGrid, v => v.CustomPropertyAddGrid.IsVisible);
			this.Bind(ViewModel, vm => vm.CustomPropertyName, v => v.CustomPropertyNameEntry.Text);
			this.Bind(ViewModel, vm => vm.CustomPropertyValue, v => v.CustomPropertyValueEntry.Text);
			this.OneWayBind(ViewModel, vm => vm.CustomProperties, v => v.CustomPropertiesListView.ItemsSource);

				this.BindCommand(ViewModel, vm => vm.Save, v => v.SaveToolbarItem);
				this.BindCommand(ViewModel, vm => vm.ToggleShowAddGrid, v => v.CustomPropertyToggleAddGridButton);
				this.BindCommand(ViewModel, vm => vm.AddProperty, v => v.CustomPropertyAddButton);
				Observable
					.FromEventPattern<ItemTappedEventArgs>(CustomPropertiesListView, nameof(ListView.ItemTapped))
					.Select(e => e.EventArgs.Item)
					.Cast<CustomPropertyViewModel>()
					.Subscribe(item => Observable.Return(Unit.Default).InvokeCommand(item.Delete));
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

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(MaterialFormViewViewModel), typeof(MaterialFormView), null);

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (MaterialFormViewViewModel)value; }
		}
	}
}
