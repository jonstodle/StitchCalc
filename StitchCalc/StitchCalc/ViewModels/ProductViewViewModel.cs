﻿using ReactiveUI;
using StitchCalc.Services.NavigationService;
using StitchCalc.Views;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using StitchCalc.Models;

namespace StitchCalc.ViewModels
{
	public class ProductViewViewModel : ViewModelBase, INavigable
	{
		readonly ReactiveList<Page> pages;
		readonly ObservableAsPropertyHelper<string> pageTitle;
		Product model;
		int selectedPageIndex;

		public ProductViewViewModel()
		{
			pages = new ReactiveList<Page>();
			pages.Add(new ProductSummaryView());
			pages.Add(new ProductMaterialsView());
			pages.Add(new ProductWorkUnitsView());

			DataService.Current.GetProducts()
				.Changed
				.Select(_ => DataService.Current.GetProduct(model.Model.Id))
				.Subscribe(newModel => Model = newModel);

			Observable.Merge(
				pages.Changed.Select(_ => 0),
				this.WhenAnyValue(x=> x.Model).Where(x => x != null).Select(_ => 0))
				.Subscribe(_ => SetModelForChildViews());

			this
				.WhenAnyValue(x => x.Model, x => x?.Name)
				.ToProperty(this, x => x.PageTitle, out pageTitle);
		}

		public ReactiveList<Page> Pages => pages;

		public string PageTitle => pageTitle.Value;

		public Product Model
		{
			get { return model; }
			set { this.RaiseAndSetIfChanged(ref model, value); }
		}

		public int SelectedPageIndex
		{
			get { return selectedPageIndex; }
			set { this.RaiseAndSetIfChanged(ref selectedPageIndex, value); }
		}

		void SetModelForChildViews()
		{
			foreach (var page in pages)
			{
				((page as IViewFor).ViewModel as INavigable).OnNavigatedTo(model.Model.Id, NavigationDirection.Forwards);
			}
		}

		public Task OnNavigatedTo(object parameter, NavigationDirection direction)
		{
			if (parameter is Guid)
			{
				Model = DataService.Current.GetProduct((Guid)parameter);
			}
			else if (parameter is Tuple<Guid,int>)
			{
				var p = parameter as Tuple<Guid, int>;

				Model = DataService.Current.GetProduct(p.Item1);
				SelectedPageIndex = p.Item2;
			}

			return Task.CompletedTask;
		}

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}