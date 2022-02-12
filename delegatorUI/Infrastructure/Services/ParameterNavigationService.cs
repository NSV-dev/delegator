using delegatorUI.Infrastructure.Stores;
using delegatorUI.ViewModel.Base;
using System;

namespace delegatorUI.Infrastructure.Services
{
    public class ParameterNavigationService<TParam, TViewModel>
        where TViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TParam, TViewModel> _createViewModel;

        public ParameterNavigationService(NavigationStore navigationStore, Func<TParam, TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate(TParam param)
        {
            TViewModel vm = _createViewModel(param);
            _navigationStore.Title = "delegator | " + vm.Title;
            _navigationStore.CurrentViewModel = vm;
        }
    }
}
