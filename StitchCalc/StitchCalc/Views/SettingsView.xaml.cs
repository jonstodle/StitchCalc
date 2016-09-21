using ReactiveUI;
using StitchCalc.ViewModels.Views;

using Xamarin.Forms;

namespace StitchCalc.Views
{
	public partial class SettingsView : ContentPage, IViewFor<SettingsViewViewModel>
	{
		public SettingsView ()
		{
			InitializeComponent ();

			ViewModel = new SettingsViewViewModel();

			this.Bind(ViewModel, vm => vm.DefaultHourlyCharge, v => v.DefaultHourlyChargeEntry.Text);
			this.OneWayBind(ViewModel, vm => vm.AppVersion, v => v.AppVersionLabel.Text);
		}

		public SettingsViewViewModel ViewModel
		{
			get { return (SettingsViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(SettingsViewViewModel), typeof(SettingsView), null);

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (SettingsViewViewModel)value; }
		}
	}
}
