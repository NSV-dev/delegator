using delegatorUI.Infrastructure.Commands.Base;
using delegatorUI.Infrastructure.Interfaces;
using delegatorUI.Infrastructure.Services;
using delegatorUI.Infrastructure.Stores;
using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace delegatorUI.ViewModel.UserControlViewModels.SharedViewModels
{
    public class AccControlViewModel : BaseViewModel, IError
    {
        private readonly APIHelper _apiHelper;
        private readonly CompanyUserStore _companyUserStore;
        private readonly NavigationService<EmpControlViewModel> _toEmp;
        private readonly NavigationService<AdminControlViewModel> _toAdmin;

        #region Error
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

        #region Change Password
        private string _oldPassword = "";
        public string OldPassword
        {
            get => _oldPassword;
            set => OnPropertyChanged(ref _oldPassword, value);
        }

        private string _newPassword = "";
        public string NewPassword
        {
            get => _newPassword;
            set => OnPropertyChanged(ref _newPassword, value);
        }

        private string _confirmPassword = "";
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => OnPropertyChanged(ref _confirmPassword, value);
        }

        public ICommand ChangePasswordCommand { get; }
        private async void OnChangePasswordCommandExecute(object p)
        {
            if (OldPassword != StringCipher.Decrypt(_companyUserStore.CompanyUser.User.Password, "delegator"))
            {
                (this as IError).Error("Старый пароль введен неверно");
                return;
            }

            _companyUserStore.CompanyUser.User.Password = StringCipher.Encrypt(ConfirmPassword, "delegator");
            await _apiHelper.Users.Update(_companyUserStore.CompanyUser.User);

            OldPassword = "";
            NewPassword = "";
            ConfirmPassword = "";
        }
        #endregion

        #region Company Selection
        private List<Company> _companies;
        public List<Company> Companies
        {
            get => _companies;
            set => OnPropertyChanged(ref _companies, value);
        }

        public ICommand ChangeCompanyCommand { get; }
        private async void OnChangeCompanyCommandExecute(object p)
        {
            await _companyUserStore.LoadCompanyUser((p as Company).Id, _companyUserStore.CompanyUser.AppUserId);
            if (_companyUserStore.CompanyUser.Role.Title == "User")
                _toEmp.Navigate();
            if (_companyUserStore.CompanyUser.Role.Title == "Admin")
                _toAdmin.Navigate();
        }
        #endregion

        #region Adding Company
        private Company _selectedCompany;

        private int _addCompanyZIndex = -1;
        public int AddCompanyZIndex
        {
            get => _addCompanyZIndex;
            set => OnPropertyChanged(ref _addCompanyZIndex, value);
        }

        private int _confirmCompanyZIndex = -1;
        public int ConfirmCompanyZIndex
        {
            get => _confirmCompanyZIndex;
            set => OnPropertyChanged(ref _confirmCompanyZIndex, value);
        }

        private int _companyDataZIndex = -1;
        public int CompanyDataZIndex
        {
            get => _companyDataZIndex;
            set => OnPropertyChanged(ref _companyDataZIndex, value);
        }

        private List<Company> _companiesList;
        public List<Company> CompaniesList
        {
            get => _companiesList;
            set => OnPropertyChanged(ref _companiesList, value);
        }

        private string _companyTitleToSearch;
        public string CompanyTitleToSearch
        {
            get => _companyTitleToSearch;
            set
            {
                OnPropertyChanged(ref _companyTitleToSearch, value);
                UpdateCompanies(_companyTitleToSearch);
            }
        }

        private string _selectedCompanyCode;
        public string SelectedCompanyCode
        {
            get => _selectedCompanyCode;
            set => OnPropertyChanged(ref _selectedCompanyCode, value);
        }

        private string _newCompanyTitle;
        public string NewCompanyTitle
        {
            get => _newCompanyTitle;
            set => OnPropertyChanged(ref _newCompanyTitle, value);
        }

        private string _newCompanyCode;
        public string NewCompanyCode
        {
            get => _newCompanyCode;
            set => OnPropertyChanged(ref _newCompanyCode, value);
        }

        public ICommand ToAddCompanyCommand { get; }
        private void OnToAddCompanyCommandExecute(object p)
        {
            AddCompanyZIndex = 1;
        }

        public ICommand BackFromAddCompanyCommand { get; }
        private void OnBackFromAddCompanyCommandExecute(object p)
        {
            AddCompanyZIndex = 0;
        }

        public ICommand CompanySelectedCommand { get; }
        private void OnCompanySelectedCommandExecute(object p)
        {
            _selectedCompany = p as Company;
            if (Companies.Where(c => c.Id == _selectedCompany.Id).Count() != 0)
            {
                (this as IError).Error("Вы уже в этой комапнии");
                return;
            }

            ConfirmCompanyZIndex = 1;
        }

        public ICommand BackFromConfirmCommand { get; }
        private void OnBackFromConfirmCommandExecute(object p)
        {
            ConfirmCompanyZIndex = -1;
        }

        public ICommand CompanyConfirmedCommand { get; }
        private async void OnCompanyConfirmedCommandExecute(object p)
        {
            if (SelectedCompanyCode != StringCipher.Decrypt(_selectedCompany.Code, "delegator"))
            {
                (this as IError).Error("Неверный код компании");
                return;
            }

            await _apiHelper.CompaniesUsers.Post(new CompanyUser()
            {
                AppUserId = _companyUserStore.CompanyUser.AppUserId,
                CompanyId = _selectedCompany.Id,
                RoleId = (await _apiHelper.Roles.GetByTitle("User")).Id
            });

            await _companyUserStore.LoadCompanyUser(_selectedCompany.Id, _companyUserStore.CompanyUser.AppUserId);
            if (_companyUserStore.CompanyUser.Role.Title == "User")
                _toEmp.Navigate();
            if (_companyUserStore.CompanyUser.Role.Title == "Admin")
                _toAdmin.Navigate();
        }

        public ICommand ToCompanyDataCommand { get; }
        private void OnToCompanyDataCommandExecute(object p)
        {
            NewCompanyTitle = "";
            NewCompanyCode = "";
            CompanyDataZIndex = 1;
        }

        public ICommand BackFromCompanyDataCommand { get; }
        private void OnBackFromCompanyDataCommandExecute(object p)
        {
            CompanyDataZIndex = -1;
        }

        public ICommand CreateCompanyCommand { get; }
        private async void OnCreateCompanyCommandExecute(object p)
        {
            await _apiHelper.Companies.Post(new Company()
            {
                Title = NewCompanyTitle,
                Code = StringCipher.Encrypt(NewCompanyCode, "delegator")
            });

            await _apiHelper.CompaniesUsers.Post(new CompanyUser()
            {
                AppUserId = _companyUserStore.CompanyUser.AppUserId,
                CompanyId = (await _apiHelper.Companies.GetByTitle(NewCompanyTitle)).Id,
                RoleId = (await _apiHelper.Roles.GetByTitle("Admin")).Id
            });

            await _companyUserStore.LoadCompanyUser((await _apiHelper.Companies.GetByTitle(NewCompanyTitle)).Id, _companyUserStore.CompanyUser.AppUserId);

            _toAdmin.Navigate();
        }

        private async void UpdateCompanies(string title) => CompaniesList = await _apiHelper.Companies.GetWhereTitleContains(title);
        #endregion

        public AccControlViewModel(APIHelper apiHelper,
            CompanyUserStore companyUserStore,
            NavigationService<EmpControlViewModel> toEmp,
            NavigationService<AdminControlViewModel> toAdmin)
        {
            _apiHelper = apiHelper;
            _companyUserStore = companyUserStore;
            _toEmp = toEmp;
            _toAdmin = toAdmin;

            ChangePasswordCommand = new RelayCommand(OnChangePasswordCommandExecute, _ =>
                !string.IsNullOrWhiteSpace(OldPassword) &&
                !string.IsNullOrWhiteSpace(NewPassword) &&
                !string.IsNullOrWhiteSpace(ConfirmPassword) &&
                NewPassword == ConfirmPassword);

            ToAddCompanyCommand = new RelayCommand(OnToAddCompanyCommandExecute);
            BackFromAddCompanyCommand = new RelayCommand(OnBackFromAddCompanyCommandExecute);
            CompanySelectedCommand = new RelayCommand(OnCompanySelectedCommandExecute);
            BackFromConfirmCommand = new RelayCommand(OnBackFromConfirmCommandExecute);
            CompanyConfirmedCommand = new RelayCommand(OnCompanyConfirmedCommandExecute);
            ToCompanyDataCommand = new RelayCommand(OnToCompanyDataCommandExecute);
            BackFromCompanyDataCommand = new RelayCommand(OnBackFromCompanyDataCommandExecute);
            CreateCompanyCommand = new RelayCommand(OnCreateCompanyCommandExecute, _ =>
                !string.IsNullOrWhiteSpace(NewCompanyTitle) &&
                !string.IsNullOrWhiteSpace(NewCompanyCode));

            ChangeCompanyCommand = new RelayCommand(OnChangeCompanyCommandExecute);

            LoadCompanies();
        }

        private async void LoadCompanies()
            => Companies = await _apiHelper.Companies.GetByUserId(_companyUserStore.CompanyUser.AppUserId);
    }
}
