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

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.DefaultHourlyCharge, v => v.DefaultHourlyChargeEntry.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.AppVersion, v => v.AppVersionLabel.Text).DisposeWith(disposables);
            });
        }
    }
}
