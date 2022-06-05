using delegatorUI.Infrastructure.Commands.Base;
using delegatorUI.Infrastructure.Interfaces;
using delegatorUI.Infrastructure.Services;
using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace delegatorUI.ViewModel.UserControlViewModels.EmpControlViewModels
{
    public class TasksControlViewModel : BaseViewModel, ILoading, IError
    {
        private readonly APIHelper _apiHelper;
        private readonly AppUser _user;
        private readonly Company _company;
        private readonly CompanyUser _companyUser;

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

        #region Errors
        private string _errorText;
        public string ErrorText
        {
            get => _errorText;
            set => OnPropertyChanged(ref _errorText, value);
        }

        private int _errorOpacity;
        public int ErrorOpacity
        {
            get => _errorOpacity;
            set => OnPropertyChanged(ref _errorOpacity, value);
        }
        #endregion

        public ICommand ReloadTasksCommand { get; }
        private async void OnReloadTasksCommandExecute(object p)
        {
            SearchText = "";
            await LoadTasks();
            await LoadTasksForToday();
        }

        #region Reports
        private AppTask _taskToReport;

        private int _reportsZIndex = -1;
        public int ReportsZIndex
        {
            get => _reportsZIndex;
            set => OnPropertyChanged(ref _reportsZIndex, value);
        }

        private string _reportTaskName;
        public string ReportTaskName
        {
            get => _reportTaskName;
            set => OnPropertyChanged(ref _reportTaskName, value);
        }

        private string _reportSenderName;
        public string ReportSenderName
        {
            get => _reportSenderName;
            set => OnPropertyChanged(ref _reportSenderName, value);
        }

        private string _reportText;
        public string ReportText
        {
            get => _reportText;
            set => OnPropertyChanged(ref _reportText, value);
        }

        private string _reportDuration;
        public string ReportDuration
        {
            get => _reportDuration;
            set => OnPropertyChanged(ref _reportDuration, value);
        }

        private ObservableCollection<OpenFileDialog> _reportFiles;
        public ObservableCollection<OpenFileDialog> ReportFiles
        {
            get => _reportFiles;
            set => OnPropertyChanged(ref _reportFiles, value);
        }

        public ICommand ToReportCommand { get; }
        private void OnToReportCommandExecute(object p)
        {
            ReportsZIndex = 1;
            _taskToReport = p as AppTask;
            ReportTaskName = (p as AppTask).Title;
            ReportSenderName = (p as AppTask).Sender.UserName;
            ReportText = "";
            ReportDuration = "";
            ReportFiles = new();
        }
        private bool CanToReportCommandExecute(object p)
        {
            if ((p as AppTask).ResponsibleId == _user.Id && (p as AppTask).Users.Count > 1)
            {
                (this as IError).Error("Сначала все пользователи должны отправить отчеты");
                return false;
            }

            return true;
        }

        public ICommand BackFromReportCommand { get; }
        private void OnBackFromReportCommandExecute(object p)
        {
            ReportsZIndex = -1;
        }

        public ICommand AddFileCommand { get; }
        private void OnAddFileCommandExecute(object p)
        {
            OpenFileDialog file = new();
            if (file.ShowDialog() == true)
            {
                try
                {
                    File.OpenRead(file.FileName);
                }
                catch
                {
                    (this as IError).Error("Нет доступа к файлу");
                    return;
                }
                ReportFiles.Add(file);
            }
        }

        public ICommand DeleteFileCommand { get; }
        private void OnDeleteFileCommandExecute(object p)
        {
            ReportFiles.Remove(p as OpenFileDialog);
        }

        public ICommand ReportCommand { get; }
        private async void OnReportCommandExecute(object p)
        {
            (this as ILoading).StartLoading();
            double size = 0;
            foreach (var file in ReportFiles)
                size += new FileInfo(file.FileName).Length;
            if (size >= 26214400)
            {
                (this as IError).Error("Объем файлов не должен превышать 25МБ");
                (this as ILoading).EndLoading();
                return;
            }

            List<string> fileIDs = new();
            foreach (var file in ReportFiles)
                fileIDs.Add
                    (
                        (await _apiHelper.AppFile.Post(new AppFile()
                        {
                            Name = file.SafeFileName,
                            Content = File.ReadAllBytes(file.FileName)
                        })).Trim('"', '\\')
                    );

            string complitedID = (await _apiHelper.Complited.Post(new Complited()
            {
                CompanyUserId = _companyUser.Id,
                Duration = int.Parse(ReportDuration),
                EndTime = DateTime.Now,
                Comment = ReportText,
                TaskCode = _taskToReport.Id,
                TaskTitle = _taskToReport.Title,
                TaskDescription = _taskToReport.Description
            })).Trim('"', '\\');

            foreach (var id in fileIDs)
                await _apiHelper.ComplitedFile.Post(new ComplitedFile()
                {
                    ComplitedId = complitedID,
                    FileId = id
                });

            EmailService.SendEmail(_taskToReport.Sender.Email, _taskToReport.Sender.UserName,
                _user.UserName, _taskToReport.Title, ReportText, ReportDuration, new List<OpenFileDialog>(ReportFiles));

            await _apiHelper.Users.DeleteTask(_taskToReport.Id, _user.Id, _company.Id);

            if (_taskToReport.Users.Count == 1)
                await _apiHelper.Tasks.Delete(_taskToReport, _company.Id);

            await LoadTasks();
            await LoadTasksForToday();
            (this as ILoading).EndLoading();
            ReportsZIndex = -1;
        }
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
                LoadTasksForToday();
            }
        }
        #endregion

        #region Changing TaskUsers
        private int _changingUsersZIndex = -1;
        public int ChangingUsersZIndex
        {
            get => _changingUsersZIndex;
            set => OnPropertyChanged(ref _changingUsersZIndex, value);
        }

        private int _addTaskUserZIndex = -1;
        public int AddTaskUserZIndex
        {
            get => _addTaskUserZIndex;
            set => OnPropertyChanged(ref _addTaskUserZIndex, value);
        }

        private int _toDoZIndex = -1;
        public int ToDoZIndex
        {
            get => _toDoZIndex;
            set => OnPropertyChanged(ref _toDoZIndex, value);
        }

        private AppTask _taskToChange;

        private AppUser _userToAdd;

        private ObservableCollection<UserWithToDo> _taskUsers = new();
        public ObservableCollection<UserWithToDo> TaskUsers
        {
            get => _taskUsers;
            set => OnPropertyChanged(ref _taskUsers, value);
        }

        private ObservableCollection<AppUser> _companyUsers;
        public ObservableCollection<AppUser> CompanyUsers
        {
            get => _companyUsers;
            set => OnPropertyChanged(ref _companyUsers, value);
        }

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

        private string _toDoText;
        public string ToDoText
        {
            get => _toDoText;
            set => OnPropertyChanged(ref _toDoText, value);
        }

        public ICommand ToChangeUsersCommand { get; }
        private void OnToChangeUsersCommandExecute(object p)
        {
            if (_user.Id != (p as AppTask).ResponsibleId)
                return;
            
            _taskToChange = p as AppTask;
            LoadUsers();
            TaskUsers = new((p as AppTask).Users);
            ChangingUsersZIndex = 1;
        }

        public ICommand BackFromChangeUsersCommand { get; }
        private void OnBackFromChangeUsersCommandExecute(object p)
        {
            ChangingUsersZIndex = -1;
        }

        public ICommand ToAddTaskUserCommand { get; }

        public ICommand BackFromAddTaskUserCommand { get; }
        private void OnBackFromAddTaskUserCommandExecute(object p)
        {
            AddTaskUserZIndex = -1;
        }

        public ICommand ToToDoTaskUserCommand { get; }
        private void OnToToDoTaskUserCommandExecute(object p)
        {
            _userToAdd = p as AppUser;
            ToDoText = "";
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
            TaskUsers.Add(new() { User = _userToAdd, ToDo = ToDoText });
            CompanyUsers.Remove(CompanyUsers.Single(u => u.Id == _userToAdd.Id));
            ToDoZIndex = -1;
            AddTaskUserZIndex = -1;
        }

        public ICommand DeleteUserCommand { get; }
        private void OnDeleteUserCommandExecute(object p)
        {
            var user = (p as UserWithToDo).User;

            if (user.Id == _user.Id)
                return;

            TaskUsers.Remove(TaskUsers.Where(u => u.User.Id == user.Id).Single());
            CompanyUsers.Add(user);
        }

        public ICommand ChangeUsersCommand { get; }
        private async void OnChangeUsersCommandExecute(object p)
        {
            (this as ILoading).StartLoading();

            AppTask _taskChanged = new()
            {
                Id = _taskToChange.Id,
                Title = _taskToChange.Title,
                Description = _taskToChange.Description,
                EndTime = _taskToChange.EndTime,
                Sender = _taskToChange.Sender,
                SenderId = _taskToChange.SenderId,
                Category = _taskToChange.Category,
                CategoryId = _taskToChange.CategoryId,
                Responsible = _taskToChange.Responsible,
                ResponsibleId = _taskToChange.ResponsibleId,
                Tasks = _taskToChange.Tasks,
                Users = TaskUsers.ToList()
            };

            await _apiHelper.Tasks.Update(_taskToChange, _taskChanged, _company.Id);
            LoadTasks();
            LoadTasksForToday();

            (this as ILoading).EndLoading();
            ChangingUsersZIndex = -1;
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
        #endregion

        public TasksControlViewModel(APIHelper apiHelper, CompanyUser companyUser)
        {
            _apiHelper = apiHelper;
            _user = companyUser.User;
            _company = companyUser.Company;
            _companyUser = companyUser;

            ReloadTasksCommand = new RelayCommand(OnReloadTasksCommandExecute);

            ToReportCommand = new RelayCommand(OnToReportCommandExecute, CanToReportCommandExecute);
            BackFromReportCommand = new RelayCommand(OnBackFromReportCommandExecute);
            AddFileCommand = new RelayCommand(OnAddFileCommandExecute);
            DeleteFileCommand = new RelayCommand(OnDeleteFileCommandExecute);
            ReportCommand = new RelayCommand(OnReportCommandExecute, _ => int.TryParse(ReportDuration, out int __));

            ToChangeUsersCommand = new RelayCommand(OnToChangeUsersCommandExecute);
            BackFromChangeUsersCommand = new RelayCommand(OnBackFromChangeUsersCommandExecute);
            ToAddTaskUserCommand = new RelayCommand(_ => AddTaskUserZIndex = 1);
            BackFromAddTaskUserCommand = new RelayCommand(OnBackFromAddTaskUserCommandExecute);
            ToToDoTaskUserCommand = new RelayCommand(OnToToDoTaskUserCommandExecute);
            BackFromToDoTaskUserCommand = new RelayCommand(OnBackFromToDoTaskUserCommandExecute);
            AddToDoCommand = new RelayCommand(OnAddToDoCommandExecute);
            DeleteUserCommand = new RelayCommand(OnDeleteUserCommandExecute);
            ChangeUsersCommand = new RelayCommand(OnChangeUsersCommandExecute,
                _ => TaskUsers.Count > 0);

            LoadTasks();
            LoadTasksForToday();
            LoadUsers();
        }

        private async Task LoadUsers()
        {
            (this as ILoading).StartLoading();
            CompanyUsers = new((await _apiHelper.Users.GetByCompany(_company.Id)).Where(cu => cu.Role.Title == "User"));
            foreach (var user in TaskUsers)
                CompanyUsers.Remove(CompanyUsers.Where(cu => cu.Id == user.User.Id).Single());
            (this as ILoading).EndLoading();
        }

        private async Task LoadTasks()
        {
            (this as ILoading).StartLoading();
            Tasks = await _apiHelper.Tasks.GetByUserAndCompany(_user.Id, _company.Id);
            List<AppTask> tasksToRemove = new();
            foreach (var task in Tasks)
            {
                var list = GetAllSubs(task);
                foreach (var task1 in Tasks)
                    if (list.Exists(t => t.Id == task1.Id))
                        tasksToRemove.Add(task1);
            }
            foreach (var task in tasksToRemove)
                Tasks.RemoveAll(t => t.Id == task.Id);

            Tasks = Tasks.Where(t => t.Title.ToLower().Contains(SearchText.ToLower())).ToList();
            Tasks = Tasks.OrderBy(t => t.EndTime).ToList();
            foreach (var task in Tasks)
                GetToDo(task);
            (this as ILoading).EndLoading();
        }

        private async Task LoadTasksForToday()
        {
            (this as ILoading).StartLoading();
            Category SortCategory = await _apiHelper.Categories.GetByTitle("Важно и Срочно");
            TasksForToday = (await _apiHelper.Tasks.GetByUserAndCompanyWithoutSubs(_user.Id, _company.Id))
                .Where(t => DateTime.Today == t.EndTime.Date || DateTime.Today > t.EndTime.Date || t.CategoryId == SortCategory.Id)
                .ToList();
            TasksForToday = TasksForToday.Where(t => t.Title.ToLower().Contains(SearchText.ToLower())).ToList();
            TasksForToday = TasksForToday.OrderBy(t => t.EndTime).ToList();
            foreach (var task in TasksForToday)
                GetToDo(task);
            (this as ILoading).EndLoading();
        }

        private async Task GetToDo(AppTask task)
        {
            task.ToDo = task.Users.Single(u => u.User.Id == _user.Id).ToDo;
            if (task.Tasks is not null)
                foreach (var sub in task.Tasks)
                    await GetToDo(sub);
        }

        private List<AppTask> GetAllSubs(AppTask task)
        {
            List<AppTask> res = new();
            res.AddRange(task.Tasks);
            foreach (var sub in task.Tasks)
                res.AddRange(GetAllSubs(sub));
            return res;
        }
    }
}
