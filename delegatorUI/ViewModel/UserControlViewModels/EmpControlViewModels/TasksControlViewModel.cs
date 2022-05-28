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

            //EmailService.SendEmail(_taskToReport.Sender.Email, _taskToReport.Sender.UserName,
            //    _user.UserName, _taskToReport.Title, ReportText, ReportDuration, new List<OpenFileDialog>(ReportFiles));

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

        public TasksControlViewModel(APIHelper apiHelper, CompanyUser companyUser)
        {
            _apiHelper = apiHelper;
            _user = companyUser.User;
            _company = companyUser.Company;
            _companyUser = companyUser;

            ReloadTasksCommand = new RelayCommand(OnReloadTasksCommandExecute);

            ToReportCommand = new RelayCommand(OnToReportCommandExecute);
            BackFromReportCommand = new RelayCommand(OnBackFromReportCommandExecute);
            AddFileCommand = new RelayCommand(OnAddFileCommandExecute);
            DeleteFileCommand = new RelayCommand(OnDeleteFileCommandExecute);
            ReportCommand = new RelayCommand(OnReportCommandExecute, _ => int.TryParse(ReportDuration, out int __));

            LoadTasks();
            LoadTasksForToday();
        }

        private async Task LoadTasks()
        {
            (this as ILoading).StartLoading();
            Tasks = await _apiHelper.Tasks.GetByUserAndCompanyOnlyMain(_user.Id, _company.Id);
            Tasks = Tasks.Where(t => t.Title.ToLower().Contains(SearchText.ToLower())).ToList();
            Tasks = Tasks.OrderBy(t => t.EndTime).ToList();
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
            (this as ILoading).EndLoading();
        }
    }
}
