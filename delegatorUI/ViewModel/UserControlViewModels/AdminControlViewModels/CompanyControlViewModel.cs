using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;

namespace delegatorUI.ViewModel.UserControlViewModels.AdminControlViewModels
{
    public class CompanyControlViewModel : BaseViewModel
    {
        private readonly APIHelper _apiHelper;
        private readonly User _user;
        private readonly Company _company;

        public CompanyControlViewModel(APIHelper apiHelper, CompanyUser companyUser)
        {
            _apiHelper = apiHelper;
            _user = companyUser.User;
            _company = companyUser.Company;
        }
    }
}
