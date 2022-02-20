using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delegatorUI.ViewModel.UserControlViewModels.AdminControlViewModels
{
    public class TasksControlViewModel : BaseViewModel
    {
        private readonly APIHelper _apiHelper;
        private readonly User _user;
        private readonly Company _company;

        public TasksControlViewModel(APIHelper apiHelper, CompanyUser companyUser)
        {
            _apiHelper = apiHelper;
            _user = companyUser.User;
            _company = companyUser.Company;
        }
    }
}
