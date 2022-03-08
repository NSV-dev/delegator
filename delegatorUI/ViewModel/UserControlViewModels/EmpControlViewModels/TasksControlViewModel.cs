using delegatorUI.Infrastructure.Interfaces;
using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace delegatorUI.ViewModel.UserControlViewModels.EmpControlViewModels
{
    public class TasksControlViewModel : BaseViewModel, ILoading
    {
        private readonly APIHelper _apiHelper;
        private readonly AppUser _user;
        private readonly Company _company;

        private List<AppTask> _tasks;
        public List<AppTask> Tasks
        {
            get => _tasks;
            set => OnPropertyChanged(ref _tasks, value);
        }

        #region Loading
        private int _loadingZIndex;
        public int LoadingZIndex
        {
            get => _loadingZIndex;
            set => OnPropertyChanged(ref _loadingZIndex, value);
        }
        #endregion

        public TasksControlViewModel(APIHelper apiHelper, CompanyUser companyUser)
        {
            _apiHelper = apiHelper;
            _user = companyUser.User;
            _company = companyUser.Company;

            LoadAsync();
        }

        private async Task LoadAsync()
        {
            (this as ILoading).StartLoading();
            Tasks = await _apiHelper.Tasks.GetByUserAndCompany(_user.Id, _company.Id);
            await Task.Delay(200);
            (this as ILoading).EndLoading();
        }
    }
}
