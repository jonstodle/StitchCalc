using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			this.Bind(ViewModel, vm => vm.Description, v => v.DescriptionEditor.Text);
			this.BindCommand(ViewModel, vm => vm.Save, v => v.SaveToolbarItem);
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
