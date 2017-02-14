using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System.Reactive.Disposables;

using Xamarin.Forms;
using ReactiveUI.XamForms;

namespace StitchCalc.Views
{
	public partial class SettingsView : ReactiveContentPage<SettingsViewViewModel>
	{
		public SettingsView ()
		{
			InitializeComponent ();

			ViewModel = new SettingsViewViewModel();

			this.WhenActivated(disposables => {
				this.Bind(ViewModel, vm => vm.DefaultHourlyCharge, v => v.DefaultHourlyChargeEntry.Text).DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.AppVersion, v => v.AppVersionLabel.Text).DisposeWith(disposables);
			});
		}
	}
}
