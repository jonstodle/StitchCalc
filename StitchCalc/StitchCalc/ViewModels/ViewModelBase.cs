using ReactiveUI;

namespace StitchCalc.ViewModels
{
	public abstract class ViewModelBase : ReactiveObject, ISupportsActivation
    {
        public ViewModelActivator Activator => _activator;



		protected readonly ViewModelActivator _activator = new ViewModelActivator();
    }
}
