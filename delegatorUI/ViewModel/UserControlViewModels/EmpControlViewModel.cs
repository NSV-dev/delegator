using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delegatorUI.ViewModel.UserControlViewModels
{
    public class EmpControlViewModel : BaseViewModel
    {
        private readonly APIHelper _apiHelper;
        private readonly User _user;
        private readonly Company _company;

        public EmpControlViewModel(APIHelper apiHelper, CompanyUser companyUser)
        {
            _apiHelper = apiHelper;

            _user = companyUser.User;
            _company = companyUser.Company;

            Title = _company.Title;
        }
    }
}
