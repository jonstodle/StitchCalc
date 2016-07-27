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
	public partial class SettingsView : ContentPage, IViewFor<SettingsViewViewModel>
	{
		public SettingsView ()
		{
			InitializeComponent ();

			this.Bind(ViewModel, vm => vm.DefaultHourlyCharge, v => v.DefaultHourlyChargeEntry.Text);
			this.OneWayBind(ViewModel, vm => vm.AppVersion, v => v.AppVersionLabel.Text);
		}

		public SettingsViewViewModel ViewModel
		{
			get { return (SettingsViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(SettingsViewViewModel), typeof(SettingsView), new SettingsViewViewModel());

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (SettingsViewViewModel)value; }
		}
	}
}
