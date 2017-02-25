using ReactiveUI;using System;using System.Linq;using System.Reactive.Linq;using Xamarin.Forms;using System.Reactive.Disposables;using System.Reactive;using StitchCalc.ViewModels;namespace StitchCalc.Views{
    public class HomeTabsView : TabbedPage, IViewFor<HomeTabsViewViewModel>, ICanActivate
    {
        public HomeTabsView()
        {
            ViewModel = new HomeTabsViewViewModel();

            Title = "StitchCalc";            Children.Add(new ProductsView());
            Children.Add(new MaterialsView());
            Children.Add(new SettingsView());
        }

        public HomeTabsViewViewModel ViewModel
        {
            get { return (HomeTabsViewViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(HomeTabsViewViewModel), typeof(HomeTabsView), null);

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (HomeTabsViewViewModel)value; }
        }

        public IObservable<Unit> Activated => Observable.FromEventPattern(this, nameof(this.Appearing)).ToSignal();

        public IObservable<Unit> Deactivated => Observable.FromEventPattern(this, nameof(this.Disappearing)).ToSignal();
    }}