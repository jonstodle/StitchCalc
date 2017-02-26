using System;
using StitchCalc.Models;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using StitchCalc.Services;
using StitchCalc.Views;

namespace StitchCalc.ViewModels
{
	public class MaterialViewModel : ViewModelBase
	{
		public MaterialViewModel(Material material)
		{
			_material = material;

            _edit = ReactiveCommand.CreateFromObservable(() => Observable.FromAsync(() => NavigationService.NavigateTo(new MaterialFormView(new MaterialFormViewModel(_material)))));
		}



        public ReactiveCommand Edit => _edit;

		public Material Material => _material;



        private readonly ReactiveCommand<Unit, Unit> _edit;
		private readonly Material _material;
    }
}
