using delegatorUI.Infrastructure.Commands.Base;
using delegatorUI.Infrastructure.Services;
using delegatorUI.Infrastructure.Stores;
using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using System.Collections.Generic;
using System.Windows.Input;

namespace delegatorUI.ViewModel.UserControlViewModels.AdminControlViewModels
{
    public class CompanyControlViewModel : BaseViewModel
    {
        private readonly APIHelper _apiHelper;
        private readonly CompanyUserStore _companyUserStore;

        private string _companyName;
        public string CompanyName
        {
            get => _companyName;
            set => OnPropertyChanged(ref _companyName, value);
        }

        private List<AppUser> _users;
        public List<AppUser> Users
        {
            get => _users;
            set => OnPropertyChanged(ref _users, value);
        }

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
            UserToEdit = p as AppUser;
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
            await _apiHelper.Users.UpdateRole(UserToEdit, _companyUserStore.CompanyUser.Company.Id);
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

            await _apiHelper.Companies.Update(c);

            await _companyUserStore.LoadCompanyUser(_companyUserStore.CompanyUser.Company.Id, _companyUserStore.CompanyUser.User.Id);

            CompanyName = _companyUserStore.CompanyUser.Company.Title;
            CompanyCode = StringCipher.Decrypt(_companyUserStore.CompanyUser.Company.Code, "delegator");

            EditCompZIndex = 0;
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

            CompanyName = _companyUserStore.CompanyUser.Company.Title;
            CompanyCode = StringCipher.Decrypt(_companyUserStore.CompanyUser.Company.Code, "delegator");

            LoadUsers();
            LoadRoles();
        }

        private async void LoadUsers() => Users = await _apiHelper.Users.GetByCompany(_companyUserStore.CompanyUser.Company.Id);

        private async void LoadRoles() => Roles = await _apiHelper.Roles.Get();
    }
}
