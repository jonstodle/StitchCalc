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
			this.Bind(ViewModel, vm => vm.Notes, v => v.NotesEditor.Text);

				this.BindCommand(ViewModel, vm => vm.Save, v => v.SaveToolbarItem);
				Observable
					.Merge(
					Observable.FromEventPattern(NameEntry, nameof(Entry.Completed)),
					Observable.FromEventPattern(WidthEntry, nameof(Entry.Completed)),
					Observable.FromEventPattern(PriceEntry, nameof(Entry.Completed)))
					.ToSignal()
					.InvokeCommand(ViewModel, x => x.Save);
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
