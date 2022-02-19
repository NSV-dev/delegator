using delegatorUI.Infrastructure.Services;
using delegatorUI.Infrastructure.Commands.Base;
using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using delegatorUI.Infrastructure.Interfaces;
using delegatorUI.ViewModel.UserControlViewModels.EmpControlViewModels;

namespace delegatorUI.ViewModel.UserControlViewModels
{
    public class LogInControlViewModel : BaseViewModel, IError, ILoading
    {
        private readonly APIHelper _apiHelper;
        private readonly NavigationService<RegControlViewModel> _toReg;
        private readonly ParameterNavigationService<CompanyUser, EmpControlViewModel> _toEmp;
        private readonly ParameterNavigationService<CompanyUser, AdminControlViewModel> _toAdmin;

        #region Loading
        private int _loadingOpacity = 0;
        public int LoadingOpacity
        {
            get => _loadingOpacity;
            set => OnPropertyChanged(ref _loadingOpacity, value);
        }

        private int _loadingZIndex = -2;
        public int LoadingZIndex
        {
            get => _loadingZIndex;
            set => OnPropertyChanged(ref _loadingZIndex, value);
        }
        #endregion

        #region Errors
        private int _errorOpacity;
        public int ErrorOpacity
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

        private async void Error(string errorText)
        {
            (this as ILoading).EndLoading();
            ErrorText = errorText;
            ErrorOpacity = 1;
            await Task.Delay(3000);
            ErrorOpacity = 0;
        }
        #endregion

        #region ToReg
        public ICommand ToRegCommand { get; }
        #endregion

        #region LoggingIn
        private User _userByLogin;

        private string _login;
        public string Login
        {
            get => _login;
            set => OnPropertyChanged(ref _login, value);
        }

        private string _password = "";
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
            (this as ILoading).StartLoading();
            _userByLogin = await _apiHelper.Users.GetByUsername(Login);
            if (_userByLogin == null)
            {
                Error("Такого пользователя не существует");
                return;
            }
            if (StringCipher.Decrypt(_userByLogin.Password, "delegator") != Password)
            {
                Error("Неверный пароль");
                return;
            }

            List<Company> userCompanies = await _apiHelper.Companies.GetByUserId(_userByLogin.Id);
            if (userCompanies.Count == 1)
                await UserRoleRecognition(userCompanies.First());
            (this as ILoading).EndLoading();

            CompanyWidth = 200;
            Companies = userCompanies;
        }

        private async void OnCompanySelectedCommandExecute(object p) => await UserRoleRecognition(p as Company);

        private async Task UserRoleRecognition(Company company)
        {
            (this as ILoading).StartLoading();
            List<CompanyUser> companyUserList = await _apiHelper.CompaniesUsers.GetByCompanyId(company.Id, _userByLogin.Id);
            CompanyUser companyUser = companyUserList.First();
            if (companyUser.Role.Title == "Admin")
                _toAdmin.Navigate(companyUser);
            if (companyUser.Role.Title == "User")
                _toEmp.Navigate(companyUser);
            (this as ILoading).EndLoading();
        }
        #endregion

        public LogInControlViewModel(APIHelper apiHelper,
            NavigationService<RegControlViewModel> toReg,
            ParameterNavigationService<CompanyUser, EmpControlViewModel> toEmp,
            ParameterNavigationService<CompanyUser, AdminControlViewModel> toAdmin)
        {
            Title = "Logging in";

            _apiHelper = apiHelper;
            _toReg = toReg;
            _toEmp = toEmp;
            _toAdmin = toAdmin;


            ToRegCommand = new RelayCommand(_ => _toReg.Navigate());
            LogInCommand = new RelayCommand(OnLogInCommandExecute, _ => !string.IsNullOrWhiteSpace(Login) &&
                                                                        !string.IsNullOrWhiteSpace(Password) &&
                                                                        CompanyWidth == 0);
            CompanySelectedCommand = new RelayCommand(OnCompanySelectedCommandExecute);
        }
    }
}
