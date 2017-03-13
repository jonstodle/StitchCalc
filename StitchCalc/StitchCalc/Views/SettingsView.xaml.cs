using ReactiveUI;
using System.Reactive.Disposables;

using Xamarin.Forms;
using ReactiveUI.XamForms;
using StitchCalc.ViewModels;

namespace StitchCalc.Views
{
	public partial class SettingsView : ReactiveContentPage<SettingsViewModel>
	{
		public SettingsView(SettingsViewModel viewModel)
		{
			InitializeComponent();

			ViewModel = viewModel;

			this.Bind(ViewModel, vm => vm.DefaultHourlyCharge, v => v.DefaultHourlyChargeEntry.Text);
			this.OneWayBind(ViewModel, vm => vm.AppVersion, v => v.AppVersionLabel.Text);
		}
	}
}
