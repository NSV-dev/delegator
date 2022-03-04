using delegatorUI.Infrastructure.Stores;
using delegatorUI.Library.Api;
using delegatorUI.ViewModel.Base;

namespace delegatorUI.ViewModel.UserControlViewModels.SharedViewModels
{
    public class AccControlViewModel : BaseViewModel
    {
        private readonly APIHelper _apiHelper;
        private readonly CompanyUserStore _companyUserStore;

        public AccControlViewModel(APIHelper apiHelper, CompanyUserStore companyUserStore)
        {
            _apiHelper = apiHelper;
            _companyUserStore = companyUserStore;
        }
    }
}
