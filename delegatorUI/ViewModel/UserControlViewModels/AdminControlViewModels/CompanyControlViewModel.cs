using delegatorUI.Infrastructure.Commands.Base;
using delegatorUI.Infrastructure.Interfaces;
using delegatorUI.Infrastructure.Services;
using delegatorUI.Infrastructure.Stores;
using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Input;

namespace delegatorUI.ViewModel.UserControlViewModels.AdminControlViewModels
{
    public class CompanyControlViewModel : BaseViewModel, ILoading
    {
        private readonly APIHelper _apiHelper;
        private readonly CompanyUserStore _companyUserStore;

        private string _companyName;
        public string CompanyName
        {
            get => _companyName;
            set => OnPropertyChanged(ref _companyName, value);
        }

        private ObservableCollection<AppUserWithStats> _users;
        public ObservableCollection<AppUserWithStats> Users
        {
            get => _users;
            set => OnPropertyChanged(ref _users, value);
        }

        #region Loading
        private int _loadingZIndex;
        public int LoadingZIndex
        {
            get => _loadingZIndex;
            set => OnPropertyChanged(ref _loadingZIndex, value);
        }
        #endregion

        #region User Editing
        #region Grid Props
        private int _userEditZIndex = 0;
        public int UserEditZIndex
        {
            get => _userEditZIndex;
            set => OnPropertyChanged(ref _userEditZIndex, value);
        }
        #endregion

        private AppUser _userToEdit;
        public AppUser UserToEdit
        {
            get => _userToEdit;
            set => OnPropertyChanged(ref _userToEdit, value);
        }

        private Role _editedRole;
        public Role EditedRole
        {
            get => _editedRole;
            set => OnPropertyChanged(ref _editedRole, value);
        }

        private List<Role> _roles;
        public List<Role> Roles
        {
            get => _roles;
            set => OnPropertyChanged(ref _roles, value);
        }

        public ICommand ToUpdateUserCommand { get; }
        private void OnToUpdateUserCommandExecute(object p)
        {
            var u = p as AppUserWithStats;
            if (u.Id == _companyUserStore.CompanyUser.User.Id)
                return;
            UserToEdit = new()
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Password = u.Password,
                Role = u.Role
            };
            EditedRole = _userToEdit.Role;
            UserEditZIndex = 1;
        }

        public ICommand BackFromEditUserCommand { get; }
        private void OnBackFromEditUserCommandExecute(object p)
        {
            UserEditZIndex = 0;
        }

        public ICommand ChangeRoleCommand { get; }
        private void OnChangeRoleCommandExecute(object p)
        {
            int index = Roles.FindIndex(r => r.Id == EditedRole.Id);

            if (index == Roles.Count - 1)
                index = 0;
            else
                index++;

            EditedRole = Roles[index];
        }

        public ICommand EditUserCommand { get; }
        private async void OnEditUserCommandExecute(object p)
        {
            UserToEdit.Role = EditedRole;

            (this as ILoading).StartLoading();
            await _apiHelper.Users.UpdateRole(UserToEdit, _companyUserStore.CompanyUser.Company.Id);
            (this as ILoading).EndLoading();

            LoadUsers();
            OnBackFromEditUserCommandExecute(null);
        }
        #endregion

        #region Company Description
        private string _companyCode;
        public string CompanyCode
        {
            get => _companyCode;
            set => OnPropertyChanged(ref _companyCode, value);
        }

        private int _editCompZIndex;
        public int EditCompZIndex
        {
            get => _editCompZIndex;
            set => OnPropertyChanged(ref _editCompZIndex, value);
        }

        private string _compNameToEdit;
        public string CompNameToEdit
        {
            get => _compNameToEdit;
            set => OnPropertyChanged(ref _compNameToEdit, value);
        }

        private string _compCodeToEdit;
        public string CompCodeToEdit
        {
            get => _compCodeToEdit;
            set => OnPropertyChanged(ref _compCodeToEdit, value);
        }


        public ICommand ToEditCompanyCommand { get; }
        private void OnToEditCompanyCommandExecute(object p)
        {
            CompNameToEdit = CompanyName;
            CompCodeToEdit = CompanyCode;
            EditCompZIndex = 1;
        }

        public ICommand BackFromEditCompanyCommand { get; }

        public ICommand EditCompCommand { get; }
        private async void OnEditCompCommandExecute(object p)
        {
            Company c = new()
            {
                Id = _companyUserStore.CompanyUser.Company.Id,
                Title = CompNameToEdit,
                Code = StringCipher.Encrypt(CompCodeToEdit, "delegator")
            };

            (this as ILoading).StartLoading();
            await _apiHelper.Companies.Update(c);

            await _companyUserStore.LoadCompanyUser(_companyUserStore.CompanyUser.Company.Id, _companyUserStore.CompanyUser.User.Id);

            CompanyName = _companyUserStore.CompanyUser.Company.Title;
            CompanyCode = StringCipher.Decrypt(_companyUserStore.CompanyUser.Company.Code, "delegator");
            (this as ILoading).EndLoading();

            EditCompZIndex = 0;
        }
        #endregion

        #region Delete User
        private AppUser _userToDelete;

        private string _userToDeleteName;
        public string UserToDeleteName
        {
            get => _userToDeleteName;
            set => OnPropertyChanged(ref _userToDeleteName, value);
        }

        private int _deleteUserZIndex;
        public int DeleteUserZIndex
        {
            get => _deleteUserZIndex;
            set => OnPropertyChanged(ref _deleteUserZIndex, value);
        }

        public ICommand ToDeleteUserCommand { get; }
        private void OnToDeleteUserCommandExecute(object p)
        {
            var u = p as AppUserWithStats;
            _userToDelete = new()
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Password = u.Password,
                Role = u.Role
            };
            UserToDeleteName = _userToDelete.UserName;
            if (_userToDelete.Id != _companyUserStore.CompanyUser.AppUserId)
                DeleteUserZIndex = 1;
        }

        public ICommand BackFromDeleteUserCommand { get; }
        private void OnBackFromDeleteUserCommandExecute(object p)
        {
            DeleteUserZIndex = -1;
        }

        public ICommand DeleteUserCommand { get; }
        private async void OnDeleteUserCommandExecute(object p)
        {
            (this as ILoading).StartLoading();
            await _apiHelper.CompaniesUsers.Delete(
                (await _apiHelper.CompaniesUsers.GetByCompanyId(_companyUserStore.CompanyUser.CompanyId, _userToDelete.Id)).Single());

            LoadUsers();
            (this as ILoading).EndLoading();

            DeleteUserZIndex = -1;
        }
        #endregion

        #region User Stats

        private DateTime _from = DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0));
        public DateTime From
        {
            get => _from;
            set => OnPropertyChanged(ref _from, value);
        }

        private DateTime _to = DateTime.Now;
        public DateTime To
        {
            get => _to;
            set => OnPropertyChanged(ref _to, value);
        }

        public ICommand CalcStats { get; set; }
        private async void OnCalcStatsExecute(object p)
        {
            var stats = await _apiHelper.Complited.GetByCompanyAndUserAndDate(_companyUserStore.CompanyUser.CompanyId, (p as AppUserWithStats).Id, From, To);
            Users.Where(u => u.Id == (p as AppUserWithStats).Id).Single().ComplitedCount = stats.Count();
            Users.Where(u => u.Id == (p as AppUserWithStats).Id).Single().ComplitedDuration = 0;
            stats.ForEach(a => Users.Where(u => u.Id == (p as AppUserWithStats).Id).Single().ComplitedDuration += a.Duration);
        }
        #endregion

        #region User Search
        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set
            {
                OnPropertyChanged(ref _searchText, value);
                LoadUsers();
            }
        }
        #endregion

        #region Complited
        private int _complitedZIndex = -1;
        public int ComplitedZIndex
        {
            get => _complitedZIndex;
            set => OnPropertyChanged(ref _complitedZIndex, value);
        }

        private int _complitedDescZIndex = -1;
        public int ComplitedDescZIndex
        {
            get => _complitedDescZIndex;
            set => OnPropertyChanged(ref _complitedDescZIndex, value);
        }

        private string _complitedUserName;
        public string ComplitedUserName
        {
            get => _complitedUserName;
            set => OnPropertyChanged(ref _complitedUserName, value);
        }

        private string _searchComplitedText;
        public string SearchComplitedText
        {
            get => _searchComplitedText;
            set
            {
                OnPropertyChanged(ref _searchComplitedText, value);
                LoadComplited();
            }
        }

        private string _complitedUserID;

        public ObservableCollection<Complited> Compliteds { get; set; }

        private Complited _selectedComplited;

        private string _taskCode;

        private string _taskName;
        public string TaskName
        {
            get => _taskName;
            set => OnPropertyChanged(ref _taskName, value);
        }

        private string _taskDescription;
        public string TaskDescription
        {
            get => _taskDescription;
            set => OnPropertyChanged(ref _taskDescription, value);
        }

        private string _complitedDescUserName;
        public string ComplitedDescUserName
        {
            get => _complitedDescUserName;
            set => OnPropertyChanged(ref _complitedDescUserName, value);
        }

        private int _complitedDuration;
        public int ComplitedDuration
        {
            get => _complitedDuration;
            set => OnPropertyChanged(ref _complitedDuration, value);
        }

        private DateTime _complitedEndTime;
        public DateTime ComplitedEndTime
        {
            get => _complitedEndTime;
            set => OnPropertyChanged(ref _complitedEndTime, value);
        }

        private string _complitedComment;
        public string ComplitedComment
        {
            get => _complitedComment;
            set => OnPropertyChanged(ref _complitedComment, value);
        }

        public ObservableCollection<Complited> OtherCompliteds { get; set; }

        private string _searchOtherComplitedText = "";
        public string SearchOtherComplitedText
        {
            get => _searchOtherComplitedText;
            set
            {
                OnPropertyChanged(ref _searchOtherComplitedText, value);
                LoadOtherComplited();
            }
        }

        public ICommand ToComplitedCommand { get; }
        private void OnToComplitedCommandExecute(object p)
        {
            if ((p as AppUserWithStats).Role.Title == "Admin")
                return;

            _complitedUserID = (p as AppUserWithStats).Id;
            ComplitedUserName = (p as AppUserWithStats).UserName;
            SearchComplitedText = "";

            ComplitedZIndex = 1;
        }

        public ICommand BackFromComplitedCommand { get; }
        private void OnBackFromComplitedCommandExecute(object p)
        {
            ComplitedZIndex = -1;
            ComplitedDescZIndex = -1;
        }

        public ICommand ToCompletedDescCommand { get; }
        private void OnToCompletedDescCommandExecute(object p)
        {
            _selectedComplited = p as Complited;
            _taskCode = _selectedComplited.TaskCode;
            TaskName = _selectedComplited.TaskTitle;
            TaskDescription = _selectedComplited.TaskDescription;
            ComplitedDescUserName = _selectedComplited.CompanyUser.User.UserName;
            ComplitedDuration = _selectedComplited.Duration;
            ComplitedEndTime = (DateTime)_selectedComplited.EndTime;
            ComplitedComment = _selectedComplited.Comment;
            LoadOtherComplited();
            ComplitedDescZIndex = 1;
        }

        public ICommand GetFailsCommand { get; }
        private async void OnGetFailsCommandExecute(object p)
        {
            List<AppFile> complitedFiles = (await _apiHelper.ComplitedFile.GetByComplited(_selectedComplited.Id))
                .Select(cf => cf.File).ToList();

            foreach (var file in complitedFiles)
            {
                var dlg = new SaveFileDialog()
                {
                    FileName = file.Name
                };
                if (dlg.ShowDialog() is true)
                    File.WriteAllBytes(dlg.FileName, file.Content);
            }
        }

        public ICommand OtherOpenCommand { get; }
        private void OnOtherOpenCommandExecute(object p)
        {
            _selectedComplited = p as Complited;
            ComplitedDescUserName = _selectedComplited.CompanyUser.User.UserName;
            ComplitedDuration = _selectedComplited.Duration;
            ComplitedEndTime = (DateTime)_selectedComplited.EndTime;
            ComplitedComment = _selectedComplited.Comment;
        }

        private async void LoadComplited()
        {
            (this as ILoading).StartLoading();
            Compliteds.Clear();
            var comps = (await _apiHelper.Complited.GetByCompanyAndUser(_companyUserStore.CompanyUser.CompanyId, _complitedUserID))
                .Where(c => c.TaskTitle.ToLower().Contains(SearchComplitedText.ToLower()))
                .OrderByDescending(c => c.EndTime)
                .ToList();
            foreach (Complited comp in comps)
                Compliteds.Add(comp);
            (this as ILoading).EndLoading();
        }

        private async void LoadOtherComplited()
        {
            (this as ILoading).StartLoading();
            OtherCompliteds.Clear();
            var comps = (await _apiHelper.Complited.GetByTaskCode(_taskCode))
                .Where(c => c.CompanyUser.User.UserName.ToLower().Contains(SearchOtherComplitedText.ToLower()))
                .OrderByDescending(c => c.EndTime)
                .ToList();
            foreach (Complited comp in comps)
                OtherCompliteds.Add(comp);
            (this as ILoading).EndLoading();
        }
        
        #endregion

        public CompanyControlViewModel(APIHelper apiHelper, CompanyUserStore companyUserStore)
        {
            _apiHelper = apiHelper;
            _companyUserStore = companyUserStore;
            Compliteds = new();
            OtherCompliteds = new();

            ToUpdateUserCommand = new RelayCommand(OnToUpdateUserCommandExecute);
            BackFromEditUserCommand = new RelayCommand(OnBackFromEditUserCommandExecute);
            ChangeRoleCommand = new RelayCommand(OnChangeRoleCommandExecute);
            EditUserCommand = new RelayCommand(OnEditUserCommandExecute);

            ToEditCompanyCommand = new RelayCommand(OnToEditCompanyCommandExecute);
            BackFromEditCompanyCommand = new RelayCommand(_ => EditCompZIndex = 0);
            EditCompCommand = new RelayCommand(OnEditCompCommandExecute);

            ToDeleteUserCommand = new RelayCommand(OnToDeleteUserCommandExecute);
            BackFromDeleteUserCommand = new RelayCommand(OnBackFromDeleteUserCommandExecute);
            DeleteUserCommand = new RelayCommand(OnDeleteUserCommandExecute);

            CompanyName = _companyUserStore.CompanyUser.Company.Title;
            CompanyCode = StringCipher.Decrypt(_companyUserStore.CompanyUser.Company.Code, "delegator");

            CalcStats = new RelayCommand(OnCalcStatsExecute);

            ToComplitedCommand = new RelayCommand(OnToComplitedCommandExecute);
            BackFromComplitedCommand = new RelayCommand(OnBackFromComplitedCommandExecute);
            ToCompletedDescCommand = new RelayCommand(OnToCompletedDescCommandExecute);
            GetFailsCommand = new RelayCommand(OnGetFailsCommandExecute);
            OtherOpenCommand = new RelayCommand(OnOtherOpenCommandExecute);

            LoadUsers();
            LoadRoles();
        }

        private async void LoadUsers()
        {
            (this as ILoading).StartLoading();
            Users = new();
            var users = (await _apiHelper.Users.GetByCompany(_companyUserStore.CompanyUser.Company.Id))
                .Where(u => u.UserName.ToLower().Contains(SearchText.ToLower())).ToList();
            users.ForEach(u => Users.Add(new(u)));
            (this as ILoading).EndLoading();
        }

        private async void LoadRoles()
        {
            (this as ILoading).StartLoading();
            Roles = await _apiHelper.Roles.Get();
            (this as ILoading).EndLoading();
        }
    }
}
