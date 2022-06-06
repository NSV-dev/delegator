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

        private int _changeRespZIndex = -1;
        public int ChangeRespZIndex
        {
            get => _changeRespZIndex;
            set => OnPropertyChanged(ref _changeRespZIndex, value);
        }

        private int _toDoZIndex = -1;
        public int ToDoZIndex
        {
            get => _toDoZIndex;
            set => OnPropertyChanged(ref _toDoZIndex, value);
        }
        #endregion

        #region Task Prop
        private ObservableCollection<UserWithToDo> _newTaskUsers = new();
        public ObservableCollection<UserWithToDo> NewTaskUsers
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

        private AppUser _resp;
        public AppUser Resp
        {
            get => _resp;
            set => OnPropertyChanged(ref _resp, value);
        }
        #endregion

        private ObservableCollection<AppUser> _respUsers;
        public ObservableCollection<AppUser> RespUsers
        {
            get => _respUsers;
            set => OnPropertyChanged(ref _respUsers, value);
        }

        private AppUser _userToAdd;

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

        private string _searchRespText;
        public string SearchRespText
        {
            get => _searchRespText;
            set
            {
                OnPropertyChanged(ref _searchRespText, value);
                UpadateRespUsers(_searchRespText);
            }
        }

        private string _toDoText;
        public string ToDoText
        {
            get => _toDoText;
            set => OnPropertyChanged(ref _toDoText, value);
        }

        private async void UpadateCompanyUsers(string name)
        {
            if (name == "")
                LoadUsers();
            else
            {
                await LoadUsers();
                CompanyUsers = new(CompanyUsers.Where(u => u.UserName.ToLower().Contains(name.ToLower())));
            }
        }

        private async void UpadateRespUsers(string name)
        {
            if (name == "")
                LoadRespUsers();
            else
            {
                await LoadRespUsers();
                RespUsers = new(RespUsers.Where(u => u.UserName.ToLower().Contains(name.ToLower())));
            }
        }

        public ICommand BackFromAddTaskCommand { get; }
        private async void OnBackFromAddTaskCommandExecute(object p)
        {
            await LoadUsers();
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
            NewTaskUsers.Remove(NewTaskUsers.Single(u => u.User.Id == (p as UserWithToDo).User.Id));
            CompanyUsers.Add((p as UserWithToDo).User);
        }

        public ICommand ToAddTaskUserCommand { get; }
        private void OnToAddTaskUserCommandExecute(object p)
        {
            AddTaskUserZIndex = 1;
        }

        public ICommand BackFromAddTaskUserCommand { get; }
        private void OnBackFromAddTaskUserCommandExecute(object p)
        {
            AddTaskUserZIndex = -1;
        }

        public ICommand ToToDoTaskUserCommand { get; }
        private void OnToToDoTaskUserCommandExecute(object p)
        {
            _userToAdd = p as AppUser;
            ToDoZIndex = 1;
        }

        public ICommand BackFromToDoTaskUserCommand { get; }
        private void OnBackFromToDoTaskUserCommandExecute(object p)
        {
            ToDoZIndex = -1;
        }

        public ICommand AddToDoCommand { get; }
        private void OnAddToDoCommandExecute(object p)
        {
            NewTaskUsers.Add(new() { User = _userToAdd, ToDo = ToDoText });
            CompanyUsers.Remove(CompanyUsers.Single(u => u.Id == _userToAdd.Id));
            ToDoZIndex = -1;
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
                Category = NewTaskCategory,
                Responsible = Resp
            };

            if (!task.Users.Select(u => u.User).ToList().Exists(t => t.Id == task.Responsible.Id))
                task.Users.Add(new() { User = task.Responsible, ToDo = "Ответственный" });

            (this as ILoading).StartLoading();

            if (oldUpdateTask is null)
                await _apiHelper.Tasks.Post(task, _company.Id, _mainTask?.Id);
            else
                await _apiHelper.Tasks.Update(oldUpdateTask, task, _company.Id);

            if (_mainTask is not null)
            {
                AppTask oldMainTask = new()
                {
                    Id = _mainTask.Id,
                    CategoryId = _mainTask.CategoryId,
                    Category = _mainTask.Category,
                    Description = _mainTask.Description,
                    EndTime = _mainTask.EndTime,
                    SenderId = _mainTask.SenderId,
                    Sender = _mainTask.Sender,
                    Title = _mainTask.Title,
                    Tasks = _mainTask.Tasks,
                    Users = new(_mainTask.Users),
                    ResponsibleId = _mainTask.ResponsibleId,
                    Responsible = _mainTask.Responsible
                };

                await _apiHelper.Tasks.Update(oldMainTask, _mainTask, _company.Id);
            }

            AddTaskZIndex = -1;
            AddTaskUserZIndex = -1;
            NewTaskTitle = "";
            NewTaskDesc = "";
            NewTaskEndDate = DateTime.Today;
            NewTaskCategory = await _apiHelper.Categories.GetByTitle("Не важно и Не срочно");
            Resp = null;
            oldUpdateTask = null;
            _mainTask = null;
            LoadUsers();
            LoadRespUsers();
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

        public ICommand ToChangeRespCommand { get; }
        private void OnToChangeRespCommandExecute(object p)
        {
            ChangeRespZIndex = 1;
        }

        public ICommand BackFromChangeRespCommand { get; }
        private void OnBackFromChangeRespCommandExecute(object p)
        {
            if (p is not null)
            {
                Resp = (AppUser)p;
            }
            ChangeRespZIndex = -10;
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
            Resp = (p as AppTask).Responsible;

            NewTaskUsers = new((p as AppTask).Users);
            foreach (var user in NewTaskUsers)
                if (CompanyUsers.Where(u => u.Id == user.User.Id).Count() == 1)
                    CompanyUsers.Remove(CompanyUsers.Where(u => u.Id == user.User.Id).Single());

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

        #region Search
        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set
            {
                OnPropertyChanged(ref _searchText, value);
                LoadTasks();
            }
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
            ToToDoTaskUserCommand = new RelayCommand(OnToToDoTaskUserCommandExecute);
            BackFromToDoTaskUserCommand = new RelayCommand(OnBackFromToDoTaskUserCommandExecute);
            AddToDoCommand = new RelayCommand(OnAddToDoCommandExecute);
            AddTaskCommand = new RelayCommand(OnAddTaskCommandExecute, _ =>
                !string.IsNullOrWhiteSpace(NewTaskTitle) &&
                !string.IsNullOrWhiteSpace(NewTaskDesc) &&
                Resp is not null);
            ReloadTasksCommand = new RelayCommand(_ => LoadTasks());

            ToDelTaskCommand = new RelayCommand(OnToDelTaskCommandExecute);
            BackFromDelTaskCommand = new RelayCommand(OnBackFromDelTaskCommandExecute);
            DelTaskCommand = new RelayCommand(OnDelTaskCommandExecute);
            ToUpdateTaskCommand = new RelayCommand(OnToUpdateTaskCommandExecute);
            ChangeCategoryCommand = new RelayCommand(OnChangeCategoryCommandExecute);
            ToChangeRespCommand = new RelayCommand(OnToChangeRespCommandExecute);
            BackFromChangeRespCommand = new RelayCommand(OnBackFromChangeRespCommandExecute);

            LoadTasks();
            LoadCategories();
            LoadUsers();
            LoadRespUsers();
        }

        private async void LoadTasks()
        {
            (this as ILoading).StartLoading();
            Tasks = await _apiHelper.Tasks.GetByCompany(_company.Id);
            Tasks = Tasks.Where(t => t.Title.ToLower().Contains(SearchText.ToLower())).ToList();
            await Task.Delay(200);
            (this as ILoading).EndLoading();
        }

        private async Task LoadUsers()
        {
            (this as ILoading).StartLoading();
            CompanyUsers = new((await _apiHelper.Users.GetByCompany(_company.Id)).Where(cu => cu.Role.Title == "User"));
            foreach (var user in NewTaskUsers)
                CompanyUsers.Remove(CompanyUsers.Where(cu => cu.Id == user.User.Id).Single());
            (this as ILoading).EndLoading();
        }

        private async Task LoadRespUsers()
        {
            (this as ILoading).StartLoading();
            RespUsers = new((await _apiHelper.Users.GetByCompany(_company.Id)).Where(cu => cu.Role.Title == "User"));
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
