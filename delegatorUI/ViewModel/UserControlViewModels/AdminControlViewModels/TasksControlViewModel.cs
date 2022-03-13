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
        private readonly AppUser _user;
        private readonly Company _company;

        public string CompanyName { get; }

        private List<AppTask> _tasks;
        public List<AppTask> Tasks
        {
            get => _tasks;
            set => OnPropertyChanged(ref _tasks, value);
        }

        private List<Category> _categories;
        public List<Category> Categories
        {
            get => _categories;
            set => OnPropertyChanged(ref _categories, value);
        }

        private ObservableCollection<AppUser> _companyUsers;
        public ObservableCollection<AppUser> CompanyUsers
        {
            get => _companyUsers;
            set => OnPropertyChanged(ref _companyUsers, value);
        }

        #region Loading
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

        #region Adding and Updating Tasks
        #region Shared
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

        #region Task Prop
        private ObservableCollection<AppUser> _newTaskUsers = new();
        public ObservableCollection<AppUser> NewTaskUsers
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

        private Category _newTaskCategory;
        public Category NewTaskCategory
        {
            get => _newTaskCategory;
            set => OnPropertyChanged(ref _newTaskCategory, value);
        }
        #endregion

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

        public ICommand BackFromAddTaskCommand { get; }
        private async void OnBackFromAddTaskCommandExecute(object p)
        {
            CompanyUsers = new(await _apiHelper.Users.GetByCompany(_company.Id));
            _mainTask = null;
            NewTaskTitle = "";
            NewTaskDesc = "";
            NewTaskEndDate = DateTime.Today;
            NewTaskCategory = await _apiHelper.Categories.GetByTitle("Не важно и Не срочно");
            NewTaskUsers = new();
            oldUpdateTask = null;
            AddTaskZIndex = -1;
        }

        public ICommand DeleteUserCommand { get; }
        private void OnDeleteUserCommandExecute(object p)
        {
            NewTaskUsers.Remove(p as AppUser);
            CompanyUsers.Add(p as AppUser);
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
                NewTaskUsers.Add(p as AppUser);
                CompanyUsers.Remove(CompanyUsers.Where(cu => cu.Id == (p as AppUser).Id).Single());
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
                Users = new(NewTaskUsers),
                Sender = _user,
                Category = NewTaskCategory
            };

            (this as ILoading).StartLoading();

            if (oldUpdateTask == null)
                await _apiHelper.Tasks.Post(task, _company.Id, _mainTask?.Id);
            else
                await _apiHelper.Tasks.Update(oldUpdateTask, task, _company.Id);

            AddTaskZIndex = -1;
            AddTaskUserZIndex = -1;
            NewTaskTitle = "";
            NewTaskDesc = "";
            NewTaskEndDate = DateTime.Today;
            NewTaskCategory = await _apiHelper.Categories.GetByTitle("Не важно и Не срочно");
            oldUpdateTask = null;
            _mainTask = null;
            foreach (AppUser user in NewTaskUsers)
                CompanyUsers.Add(user);
            NewTaskUsers = new();
            LoadTasks();
            (this as ILoading).EndLoading();
        }

        public ICommand ChangeCategoryCommand{ get; }
        private void OnChangeCategoryCommandExecute(object p)
        {
            int index = Categories.FindIndex(c => c.Id == NewTaskCategory.Id);

            if (index == Categories.Count - 1)
                index = 0;
            else
                index++;

            NewTaskCategory = Categories[index];
        }
        #endregion

        #region Adding Tasks
        private AppTask _mainTask;

        public ICommand ToAddTaskCommand { get; }
        private void OnToAddTaskCommandExecute(object p)
        {
            _mainTask = p as AppTask;
            AddTaskZIndex = 1;
        }
        #endregion

        #region Updating Tasks
        private AppTask oldUpdateTask;

        public ICommand ToUpdateTaskCommand { get; }
        private void OnToUpdateTaskCommandExecute(object p)
        {
            NewTaskTitle = (p as AppTask).Title;
            NewTaskDesc = (p as AppTask).Description;
            NewTaskEndDate = (DateTime)(p as AppTask).EndTime;
            NewTaskCategory = (p as AppTask).Category;

            NewTaskUsers = new((p as AppTask).Users);
            foreach (AppUser user in NewTaskUsers)
                if (CompanyUsers.Where(u => u.Id == user.Id).Count() == 1)
                    CompanyUsers.Remove(CompanyUsers.Where(u => u.Id == user.Id).Single());

            oldUpdateTask = new()
            {
                Id = (p as AppTask).Id,
                Title = (p as AppTask).Title,
                Description = (p as AppTask).Description,
                EndTime = (p as AppTask).EndTime,
                Users = (p as AppTask).Users
            };

            AddTaskZIndex = 1;
        }
        #endregion
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
            ToUpdateTaskCommand = new RelayCommand(OnToUpdateTaskCommandExecute);
            ChangeCategoryCommand = new RelayCommand(OnChangeCategoryCommandExecute);

            LoadTasks();
            LoadCategories();
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

        private async void LoadCategories()
        {
            (this as ILoading).StartLoading();
            NewTaskCategory = await _apiHelper.Categories.GetByTitle("Не важно и Не срочно");
            Categories = await _apiHelper.Categories.GetAll();
            (this as ILoading).EndLoading();
        }
    }
}
