﻿using StitchCalc.Services.NavigationService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels.Views
{
	public class ProductMaterialsViewViewModel : ViewModelBase, INavigable
	{
		public Task OnNavigatedTo(object parameter, NavigationDirection direction) => Task.CompletedTask;

		public Task OnNavigatingFrom() => Task.CompletedTask;
	}
}
