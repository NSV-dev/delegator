using delegatorUI.Infrastructure.Commands.Base;
using delegatorUI.Infrastructure.Interfaces;
using delegatorUI.Infrastructure.Services;
using delegatorUI.Infrastructure.Stores;
using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            var stats = await _apiHelper.Complited.GetByUserAndDate((p as AppUserWithStats).Id, From, To);
            Users.Where(u => u.Id == (p as AppUserWithStats).Id).Single().ComplitedCount = stats.Count();
            Users.Where(u => u.Id == (p as AppUserWithStats).Id).Single().ComplitedDuration = 0;
            stats.ForEach(a => Users.Where(u => u.Id == (p as AppUserWithStats).Id).Single().ComplitedDuration += a.Duration);
        }
        #endregion

        public CompanyControlViewModel(APIHelper apiHelper, CompanyUserStore companyUserStore)
        {
            _apiHelper = apiHelper;
            _companyUserStore = companyUserStore;

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

            LoadUsers();
            LoadRoles();
        }

        private async void LoadUsers()
        {
            (this as ILoading).StartLoading();
            Users = new();
            var users = await _apiHelper.Users.GetByCompany(_companyUserStore.CompanyUser.Company.Id);
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
