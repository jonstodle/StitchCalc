using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System;
using System.Linq;
using System.Reactive.Linq;

using Xamarin.Forms;

namespace StitchCalc.Views
{
	public class ProductView : TabbedPage, IViewFor<ProductViewViewModel>
	{
		public ProductView ()
		{
			ViewModel = new ProductViewViewModel();

			this.OneWayBind(ViewModel, vm => vm.PageTitle, v => v.Title);

				ViewModel.Pages
					.Changed
					.Throttle(TimeSpan.FromMilliseconds(10))
					.Select(_ => ViewModel.Pages)
					.StartWith(ViewModel.Pages)
					.Subscribe(pages =>
					{
						Children.Clear();
						foreach (var page in pages) { Children.Add(page); }
					});
				ViewModel.WhenAnyValue(x => x.SelectedPageIndex)
					.Where(x => x >= 0 && x < Children.Count)
					.Select(x => Children[x])
					.Subscribe(x => SelectedItem = x);
		}

		public ProductViewViewModel ViewModel
		{
			get { return (ProductViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(ProductViewViewModel), typeof(ProductView), null);

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ProductViewViewModel)value; }
		}
	}
}
