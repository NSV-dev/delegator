using delegatorUI.Infrastructure.Commands.Base;
using delegatorUI.Infrastructure.Stores;
using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace delegatorUI.ViewModel.UserControlViewModels
{
    public class LogInControlViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly APIHelper _apiHelper;
        private readonly AdminControlViewModel _adminControlViewModel;
        private readonly EmpControlViewModel _empControlViewModel;

        #region Errors
        private double _errorOpacity;
        public double ErrorOpacity
        {
            get => _errorOpacity;
            set => OnPropertyChanged(ref _errorOpacity, value);
        }

        private string _errorText;
        public string ErrorText
        {
            get => _errorText;
            set => OnPropertyChanged(ref _errorText, value);
        }

        private async void ShowError(string errorText)
        {
            ErrorText = errorText;
            ErrorOpacity = 1;
            await Task.Delay(3000);
            ErrorOpacity = 0;
        }
        #endregion

        #region ToReg
        private readonly RegControlViewModel _regControlViewModel;

        public ICommand ToRegCommand { get; }

        private void OnToRegCommandExecute(object p)
        {
            _navigationStore.Title = "Register";
            _navigationStore.CurrentViewModel = _regControlViewModel;
        }
        #endregion

        #region LoggingIn
        private User _userByLogin;

        private string _login;
        public string Login
        {
            get => _login;
            set => OnPropertyChanged(ref _login, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => OnPropertyChanged(ref _password, value);
        }

        private int _companyWidth;
        public int CompanyWidth
        {
            get => _companyWidth;
            set => OnPropertyChanged(ref _companyWidth, value);
        }

        private List<Company> _companies;
        public List<Company> Companies
        {
            get => _companies;
            set => OnPropertyChanged(ref _companies, value);
        }

        public ICommand LogInCommand { get; }
        public ICommand CompanySelectedCommand { get; }

        private async void OnLogInCommandExecute(object p)
        {
            _userByLogin = await _apiHelper.Users.GetByUsername(Login);
            if (_userByLogin == null)
            {
                ShowError("Такого пользователя не существует");
                return;
            }
            if (_userByLogin.Password != Password)
            {
                ShowError("Неверный пароль");
                return;
            }

            List<Company> userCompanies = await _apiHelper.Companies.GetByUserId(_userByLogin.Id);
            if (userCompanies.Count == 1)
                await UserRoleRecognition(userCompanies.First());

            CompanyWidth = 200;
            Companies = userCompanies;
        }

        private async void OnCompanySelectedCommandExecute(object p) => await UserRoleRecognition(p as Company);

        private async Task UserRoleRecognition(Company company)
        {
            List<CompanyUser> companyUserList = await _apiHelper.CompaniesUsers.GetByCompanyId(company.Id, _userByLogin.Id);
            CompanyUser companyUser = companyUserList.First();
            _navigationStore.Title = companyUser.Company.Title;
            if (companyUser.Role.Title == "Admin")
                _navigationStore.CurrentViewModel = _adminControlViewModel;
            if (companyUser.Role.Title == "User")
                _navigationStore.CurrentViewModel = _empControlViewModel;
        }
        #endregion

        public LogInControlViewModel(NavigationStore navigationStore, APIHelper apiHelper, 
            RegControlViewModel regControlViewModel, AdminControlViewModel adminControlViewModel, 
            EmpControlViewModel empControlViewModel)
        {
            _navigationStore = navigationStore;
            _apiHelper = apiHelper;
            _regControlViewModel = regControlViewModel;
            _adminControlViewModel = adminControlViewModel;
            _empControlViewModel = empControlViewModel;

            ToRegCommand = new RelayCommand(OnToRegCommandExecute);
            LogInCommand = new RelayCommand(OnLogInCommandExecute, _ => !string.IsNullOrWhiteSpace(Login) && 
                                                                        !string.IsNullOrWhiteSpace(Password) && 
                                                                        CompanyWidth == 0);
            CompanySelectedCommand = new RelayCommand(OnCompanySelectedCommandExecute);
        }
    }
}
