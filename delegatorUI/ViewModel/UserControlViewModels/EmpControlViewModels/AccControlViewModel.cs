using delegatorUI.Infrastructure.Stores;
using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delegatorUI.ViewModel.UserControlViewModels.EmpControlViewModels
{
    public class AccControlViewModel : BaseViewModel
    {
        private readonly APIHelper _apiHelper;

        public AccControlViewModel(APIHelper apiHelper, CompanyUserStore companyUserStore)
        {
            _apiHelper = apiHelper;
        }
    }
}
