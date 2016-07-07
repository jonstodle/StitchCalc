using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StitchCalc.Services.NavigationService
{
    public interface INavigable
    {
		Task OnNavigatedTo(object parameter, NavigationDirection direction);
		Task OnNavigatingFrom();
    }
}
