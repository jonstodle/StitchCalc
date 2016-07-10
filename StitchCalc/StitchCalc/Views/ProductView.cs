using ReactiveUI;
using StitchCalc.ViewModels.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace StitchCalc.Views
{
	public class ProductView : TabbedPage, IViewFor<ProductViewViewModel>
	{
		public ProductView ()
		{
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
		}

		public ProductViewViewModel ViewModel
		{
			get { return (ProductViewViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(ProductViewViewModel), typeof(ProductView), new ProductViewViewModel());

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ProductViewViewModel)value; }
		}
	}
}
