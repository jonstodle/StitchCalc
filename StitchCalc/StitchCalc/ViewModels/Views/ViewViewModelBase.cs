using ReactiveUI;

namespace StitchCalc.ViewModels.Views
{
    public class ViewViewModelBase : ViewModelBase, ISupportsActivation
    {
        public ViewModelActivator Activator => _activator;



        protected readonly ViewModelActivator _activator = new ViewModelActivator();
    }
}