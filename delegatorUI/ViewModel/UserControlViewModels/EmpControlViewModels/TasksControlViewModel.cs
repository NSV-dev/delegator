using delegatorUI.Infrastructure.Commands.Base;
using delegatorUI.Infrastructure.Interfaces;
using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

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

        private List<AppTask> _tasksForToday;
        public List<AppTask> TasksForToday
        {
            get => _tasksForToday;
            set => OnPropertyChanged(ref _tasksForToday, value);
        }

        #region Loading
        private int _loadingZIndex;
        public int LoadingZIndex
        {
            get => _loadingZIndex;
            set => OnPropertyChanged(ref _loadingZIndex, value);
        }
        #endregion

        public ICommand ReloadTasksCommand { get; }
        private async void OnReloadTasksCommandExecute(object p)
        {
            await LoadTasks();
            await LoadTasksForToday();
        }

        public TasksControlViewModel(APIHelper apiHelper, CompanyUser companyUser)
        {
            _apiHelper = apiHelper;
            _user = companyUser.User;
            _company = companyUser.Company;

            ReloadTasksCommand = new RelayCommand(OnReloadTasksCommandExecute);

            LoadTasks();
            LoadTasksForToday();
        }

        private async Task LoadTasks()
        {
            (this as ILoading).StartLoading();
            Tasks = await _apiHelper.Tasks.GetByUserAndCompanyOnlyMain(_user.Id, _company.Id);
            (this as ILoading).EndLoading();
        }

        private async Task LoadTasksForToday()
        {
            (this as ILoading).StartLoading();
            TasksForToday = (await _apiHelper.Tasks.GetByUserAndCompanyWithoutSubs(_user.Id, _company.Id))
                .Where(t => DateTime.Today == t.EndTime.Date || DateTime.Today > t.EndTime.Date)
                .ToList();
            (this as ILoading).EndLoading();
        }
    }
}
