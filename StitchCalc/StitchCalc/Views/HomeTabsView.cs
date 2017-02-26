using ReactiveUI;using System;using System.Linq;using System.Reactive.Linq;using Xamarin.Forms;using System.Reactive.Disposables;using System.Reactive;using StitchCalc.ViewModels;namespace StitchCalc.Views{
    public class HomeTabsView : TabbedPage
    {
        public HomeTabsView()
        {
            Title = "StitchCalc";            Children.Add(new ProductsView(new ProductsViewModel()));
            Children.Add(new MaterialsView());
            Children.Add(new SettingsView(new SettingsViewModel()));
        }
    }}