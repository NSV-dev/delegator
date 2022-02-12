using delegatorUI.Infrastructure.Stores;
using delegatorUI.ViewModel.Base;
using System;

namespace delegatorUI.Infrastructure.Services
{
    public class NavigationService<TViewModel>
        where TViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TViewModel> _createViewModel;

        public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate()
        {
            TViewModel vm = _createViewModel(); 
            _navigationStore.Title = "delegator | " + vm.Title;
            _navigationStore.CurrentViewModel = vm;
        }
    }
}
