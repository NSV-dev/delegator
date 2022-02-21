using delegatorUI.Infrastructure.Commands.Base;
using delegatorUI.Infrastructure.Interfaces;
using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace delegatorUI.ViewModel.UserControlViewModels.AdminControlViewModels
{
    public class TasksControlViewModel : BaseViewModel, ILoading
    {
        private readonly APIHelper _apiHelper;
        private readonly User _user;
        private readonly Company _company;

        public string CompanyName { get; }

        private List<AppTask> _tasks;
        public List<AppTask> Tasks
        {
            get => _tasks;
            set => OnPropertyChanged(ref _tasks, value);
        }

        private ObservableCollection<User> _companyUsers;
        public ObservableCollection<User> CompanyUsers
        {
            get => _companyUsers;
            set => OnPropertyChanged(ref _companyUsers, value);
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

        #region Deleting Tasks
        #region Grid Prop
        private int _delTaskOpacity = 0;
        public int DelTaskOpacity
        {
            get => _delTaskOpacity;
            set => OnPropertyChanged(ref _delTaskOpacity, value);
        }

        private int _delTaskZIndex = -1;
        public int DelTaskZIndex
        {
            get => _delTaskZIndex;
            set => OnPropertyChanged(ref _delTaskZIndex, value);
        }
        #endregion

        private AppTask _taskToDelete;
        public AppTask TaskToDel
        {
            get => _taskToDelete;
            set => OnPropertyChanged(ref _taskToDelete, value);
        }

        public ICommand ToDelTaskCommand { get; }
        private void OnToDelTaskCommandExecute(object p)
        {
            TaskToDel = p as AppTask;
            DelTaskZIndex = 1;
        }

        public ICommand BackFromDelTaskCommand { get; }
        private void OnBackFromDelTaskCommandExecute(object p)
        {
            DelTaskZIndex = -1;
        }

        public ICommand DelTaskCommand { get; }
        private async void OnDelTaskCommandExecute(object p)
        {
            (this as ILoading).StartLoading();
            await _apiHelper.Tasks.Delete(TaskToDel, _company.Id);
            DelTaskZIndex = -1;
            LoadTasks();
            (this as ILoading).EndLoading();
        }
        #endregion

        #region Adding Tasks
        #region Grid Prop
        private int _addTaskOpacity = 0;
        public int AddTaskOpacity
        {
            get => _addTaskOpacity;
            set => OnPropertyChanged(ref _addTaskOpacity, value);
        }

        private int _addTaskZIndex = -1;
        public int AddTaskZIndex
        {
            get => _addTaskZIndex;
            set => OnPropertyChanged(ref _addTaskZIndex, value);
        }

        private int _addTaskUserOpacity = 0;
        public int AddTaskUserOpacity
        {
            get => _addTaskUserOpacity;
            set => OnPropertyChanged(ref _addTaskUserOpacity, value);
        }

        private int _addTaskUserZIndex = -1;
        public int AddTaskUserZIndex
        {
            get => _addTaskUserZIndex;
            set => OnPropertyChanged(ref _addTaskUserZIndex, value);
        }
        #endregion

        private ObservableCollection<User> _newTaskUsers = new();
        public ObservableCollection<User> NewTaskUsers
        {
            get => _newTaskUsers;
            set => OnPropertyChanged(ref _newTaskUsers, value);
        }

        private string _newTaskTitle;
        public string NewTaskTitle
        {
            get => _newTaskTitle;
            set => OnPropertyChanged(ref _newTaskTitle, value);
        }

        private string _newTaskDesc;
        public string NewTaskDesc
        {
            get => _newTaskDesc;
            set => OnPropertyChanged(ref _newTaskDesc, value);
        }

        private DateTime _newTaskEndDate = DateTime.Today;
        public DateTime NewTaskEndDate
        {
            get => _newTaskEndDate;
            set => OnPropertyChanged(ref _newTaskEndDate, value);
        }

        private AppTask _mainTask;

        private string _searchTaskText;
        public string SearchTaskText
        {
            get => _searchTaskText;
            set
            {
                OnPropertyChanged(ref _searchTaskText, value);
                UpadateCompanyUsers(_searchTaskText);
            }
        }

        private async void UpadateCompanyUsers(string name)
        {
            if (name == "")
                CompanyUsers = new(await _apiHelper.Users.GetByCompany(_company.Id));
            else
                CompanyUsers = new(await _apiHelper.Users.GetWhereNameContains(name));
        }

        public ICommand ToAddTaskCommand { get; }
        private void OnToAddTaskCommandExecute(object p)
        {
            _mainTask = p as AppTask;
            AddTaskZIndex = 1;
        }

        public ICommand BackFromAddTaskCommand { get; }
        private void OnBackFromAddTaskCommandExecute(object p)
        {
            _mainTask = null;
            AddTaskZIndex = -1;
        }

        public ICommand DeleteUserCommand { get; }
        private void OnDeleteUserCommandExecute(object p)
        {
            NewTaskUsers.Remove(p as User);
            CompanyUsers.Add(p as User);
        }

        public ICommand ToAddTaskUserCommand { get; }
        private void OnToAddTaskUserCommandExecute(object p)
        {
            AddTaskUserZIndex = 1;
        }

        public ICommand BackFromAddTaskUserCommand { get; }
        private void OnBackFromAddTaskUserCommandExecute(object p)
        {
            if (p != null)
            {
                NewTaskUsers.Add(p as User);
                CompanyUsers.Remove(CompanyUsers.Where(cu => cu.Id == (p as User).Id).Single());
            }
            AddTaskUserZIndex = -1;
        }

        public ICommand AddTaskCommand { get; }
        private async void OnAddTaskCommandExecute(object p)
        {
            AppTask task = new()
            {
                Id = Guid.NewGuid().ToString().ToUpper(),
                Title = NewTaskTitle,
                Description = NewTaskDesc,
                EndTime = NewTaskEndDate,
                Users = new(NewTaskUsers)
            };

            (this as ILoading).StartLoading();
            await _apiHelper.Tasks.Post(task, _company.Id, _mainTask?.Id);
            AddTaskZIndex = -1;
            AddTaskUserZIndex = -1;
            NewTaskTitle = "";
            NewTaskDesc = "";
            NewTaskEndDate = DateTime.Today;
            foreach (User user in NewTaskUsers)
                CompanyUsers.Add(user);
            NewTaskUsers = new();
            LoadTasks();
            (this as ILoading).EndLoading();
        }
        #endregion

        public ICommand ReloadTasksCommand { get; }

        public TasksControlViewModel(APIHelper apiHelper, CompanyUser companyUser)
        {
            _apiHelper = apiHelper;
            _user = companyUser.User;
            _company = companyUser.Company;

            CompanyName = _company.Title;

            ToAddTaskCommand = new RelayCommand(OnToAddTaskCommandExecute);
            BackFromAddTaskCommand = new RelayCommand(OnBackFromAddTaskCommandExecute);
            DeleteUserCommand = new RelayCommand(OnDeleteUserCommandExecute);
            ToAddTaskUserCommand = new RelayCommand(OnToAddTaskUserCommandExecute);
            BackFromAddTaskUserCommand = new RelayCommand(OnBackFromAddTaskUserCommandExecute);
            AddTaskCommand = new RelayCommand(OnAddTaskCommandExecute, _ =>
                !string.IsNullOrWhiteSpace(NewTaskTitle) &&
                !string.IsNullOrWhiteSpace(NewTaskDesc));
            ReloadTasksCommand = new RelayCommand(_ => LoadTasks());

            ToDelTaskCommand = new RelayCommand(OnToDelTaskCommandExecute);
            BackFromDelTaskCommand = new RelayCommand(OnBackFromDelTaskCommandExecute);
            DelTaskCommand = new RelayCommand(OnDelTaskCommandExecute);

            LoadTasks();
            LoadUsers();
        }

        private async void LoadTasks()
        {
            (this as ILoading).StartLoading();
            Tasks = await _apiHelper.Tasks.GetByCompany(_company.Id);
            await Task.Delay(200);
            (this as ILoading).EndLoading();
        }

        private async void LoadUsers()
        {
            (this as ILoading).StartLoading();
            CompanyUsers = new(await _apiHelper.Users.GetByCompany(_company.Id));
            (this as ILoading).EndLoading();
        }
    }
}
