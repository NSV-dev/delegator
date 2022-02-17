using delegatorUI.Infrastructure.Interfaces;
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
    public class EmpControlViewModel : BaseViewModel, ILoading
    {
        private readonly APIHelper _apiHelper;
        private readonly User _user;
        private readonly Company _company;

        private List<AppTask> _tasks;
        public List<AppTask> Tasks
        {
            get => _tasks;
            set => OnPropertyChanged(ref _tasks, value);
        }

        #region Loading
        private int _loadingOpacity;
        public int LoadingOpacity
        {
            get => _loadingOpacity;
            set => OnPropertyChanged(ref _loadingOpacity, value);
        }

        private int _loadingZIndex;
        public int LoadingZIndex 
        {
            get => _loadingZIndex;
            set => OnPropertyChanged(ref _loadingZIndex, value);
        }
        #endregion

        public EmpControlViewModel(APIHelper apiHelper, CompanyUser companyUser)
        {
            _apiHelper = apiHelper;

            _user = companyUser.User;
            _company = companyUser.Company;

            Title = _company.Title;

            LoadAsync();
        }

        private async Task LoadAsync()
        {
            (this as ILoading).StartLoading();
            Tasks = await _apiHelper.Tasks.GetByUserAndCompany(_user.Id, _company.Id);
            (this as ILoading).EndLoading();
        }
    }
}
