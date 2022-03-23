using delegatorUI.Infrastructure.Commands.Base;
using delegatorUI.Infrastructure.Interfaces;
using delegatorUI.Infrastructure.Services;
using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace delegatorUI.ViewModel.UserControlViewModels
{
    public class RegControlViewModel : BaseViewModel, IError, ILoading
    {
        private readonly APIHelper _apiHelper;
        private readonly NavigationService<LogInControlViewModel> _toLog;

        #region Loading
        private int _loadingZIndex = -2;
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

        private async void Error(string errorText)
        {
            (this as ILoading).EndLoading();
            ErrorText = errorText;
            ErrorOpacity = 1;
            await Task.Delay(5000);
            ErrorOpacity = 0;
        }
        #endregion

        #region ToLog
        public ICommand ToLogCommand { get; }
        #endregion

        #region Registration

        #region Properties

        #region Data Properties
        #region User Data Properties
        private string _login;
        public string Login
        {
            get => _login;
            set => OnPropertyChanged(ref _login, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => OnPropertyChanged(ref _email, value);
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set => OnPropertyChanged(ref _password, value);
        }

        private string _confirmPassrord = "";
        public string ConfirmPassword
        {
            get => _confirmPassrord;
            set => OnPropertyChanged(ref _confirmPassrord, value);
        }
        #endregion

        #region Company Data Properties
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
        #endregion
        #endregion

        #region Grids

        #region Grid's Opacities 
        private int _dataOpacity = 1;
        public int DataOpacity
        {
            get => _dataOpacity;
            set => OnPropertyChanged(ref _dataOpacity, value);
        }

        private int _companiesOpacity;
        public int CompaniesOpacity
        {
            get => _companiesOpacity;
            set => OnPropertyChanged(ref _companiesOpacity, value);
        }

        private int _companyDataOpacity;
        public int CompanyDataOpacity
        {
            get => _companyDataOpacity;
            set => OnPropertyChanged(ref _companyDataOpacity, value);
        }

        private int _companyConfirmOpacity;
        public int CompanyConfirmOpacity
        {
            get => _companyConfirmOpacity;
            set => OnPropertyChanged(ref _companyConfirmOpacity, value);
        }
        #endregion
        #region Grid's ZIndexes
        private int _dataZIndex = 0;
        public int DataZIndex
        {
            get => _dataZIndex;
            set => OnPropertyChanged(ref _dataZIndex, value);
        }

        private int _companiesZIndex = -1;
        public int CompaniesZIndex
        {
            get => _companiesZIndex;
            set => OnPropertyChanged(ref _companiesZIndex, value);
        }

        private int _companyDataZIndex = -1;
        public int CompanyDataZIndex
        {
            get => _companyDataZIndex;
            set => OnPropertyChanged(ref _companyDataZIndex, value);
        }

        private int _companyConfirmZIndex = -1;
        public int CompanyConfirmZIndex
        {
            get => _companyConfirmZIndex;
            set => OnPropertyChanged(ref _companyConfirmZIndex, value);
        }
        #endregion

        #endregion

        private string _companyTitleToSearch;
        public string CompanyTitleToSearch
        {
            get => _companyTitleToSearch;
            set
            {
                OnPropertyChanged(ref _companyTitleToSearch, value);
                UpdateCompaniesAsync();
            }
        }


        private List<Company> _companiesList;
        public List<Company> CompaniesList
        {
            get => _companiesList;
            set => OnPropertyChanged(ref _companiesList, value);
        }

        private Company _selectedCompany;

        private string _selectedCompanyCode;
        public string SelectedCompanyCode
        {
            get => _selectedCompanyCode;
            set => OnPropertyChanged(ref _selectedCompanyCode, value);
        }
        #endregion

        #region Commands
        public ICommand NextCommand { get; }
        public ICommand CompanySelectedCommand { get; }
        public ICommand NewCompanyCommand { get; }
        public ICommand BackToCompaniesCommand { get; }
        public ICommand CreateCompanyCommand { get; }
        public ICommand BackToCompaniesFromCodeCommand { get; }
        public ICommand CompanyConfirmedCommand { get; }

        private async void OnNextCommandExecute(object p)
        {
            (this as ILoading).StartLoading();
            if (!ValidationService.IsValidEmail(Email))
            {
                Error("Введите корректный email");
                return;
            }
            if (Password != ConfirmPassword)
            {
                Error("Пароли не совпадают");
                return;
            }
            if (await _apiHelper.Users.GetByUsername(Login) != null)
            {
                Error("Такой логин уже существует");
                return;
            }
            (this as ILoading).EndLoading();

            DataOpacity = 0;
            DataZIndex = -1;
            CompaniesOpacity = 1;
            CompaniesZIndex = 0;
        }

        private async void OnCompanySelectedCommandExecute(object p)
        {
            _selectedCompany = (Company)p;
            CompaniesOpacity = 0;
            CompaniesZIndex = -1;
            await Task.Delay(100);
            CompanyConfirmOpacity = 1;
            CompanyConfirmZIndex = 0;
        }

        private async void OnNewCompanyCommandExecute(object p)
        {
            CompaniesOpacity = 0;
            CompaniesZIndex = -1;
            await Task.Delay(100);
            CompanyDataOpacity = 1;
            CompanyDataZIndex = 0;
        }

        private async void OnBackToCompaniesCommandExecute(object p)
        {
            CompanyDataOpacity = 0;
            CompanyDataZIndex = -1;
            await Task.Delay(100);
            CompaniesOpacity = 1;
            CompaniesZIndex = 0;
        }

        private async void OnCreateCompanyCommandExecute(object p)
        {
            if (await _apiHelper.Companies.GetByTitle(NewCompanyTitle) != null)
            {
                Error("Такое название уже существует");
                return;
            }
            PostCompany();
        }

        private async void OnBackToCompaniesFromCodeCommandExecute(object p)
        {
            CompanyConfirmOpacity = 0;
            CompanyConfirmZIndex = -1;
            await Task.Delay(100);
            CompaniesOpacity = 1;
            CompaniesZIndex = 0;
        }

        private void OnCompanyConfirmedCommandExecute(object p)
        {
            if (StringCipher.Decrypt(_selectedCompany.Code, "delegator") == SelectedCompanyCode)
                PostUser();
            else
                Error("Код не соответсвует");
        }
        #endregion

        #region Extra Funcs
        private AppUser CreateUser()
        {
            return new AppUser()
            {
                UserName = Login,
                Email = Email,
                Password = StringCipher.Encrypt(Password, "delegator")
            };
        }

        private Company CreateCompany()
        {
            return new Company()
            {
                Title = NewCompanyTitle,
                Code = StringCipher.Encrypt(NewCompanyCode, "delegator")
            };
        }

        private async void PostUser()
        {
            AppUser createdUser = CreateUser();
            if (createdUser == null)
                return;

            await _apiHelper.Users.Post(createdUser);
            PostCompanyUser((await _apiHelper.Users.GetByUsername(createdUser.UserName)).Id);
        }

        private async void PostCompany()
        {
            AppUser createdUser = CreateUser();
            if (createdUser == null)
                return;

            Company createdCompany = CreateCompany();
            if (createdCompany == null)
                return;

            await _apiHelper.Companies.Post(createdCompany);
            await _apiHelper.Users.Post(createdUser);

            PostCompanyUser(
                (await _apiHelper.Users.GetByUsername(createdUser.UserName)).Id,
                (await _apiHelper.Companies.GetByTitle(createdCompany.Title)).Id);
        }

        private async void PostCompanyUser(string createdUserId, string createdCompanyId = null)
        {
            (this as ILoading).StartLoading();
            if (createdCompanyId != null)
                await _apiHelper.CompaniesUsers.Post(new CompanyUser()
                {
                    AppUserId = createdUserId,
                    CompanyId = createdCompanyId,
                    RoleId = (await _apiHelper.Roles.GetByTitle("Admin")).Id
                });
            else
                await _apiHelper.CompaniesUsers.Post(new CompanyUser()
                {
                    AppUserId = createdUserId,
                    CompanyId = _selectedCompany.Id,
                    RoleId = (await _apiHelper.Roles.GetByTitle("User")).Id
                });
            (this as ILoading).EndLoading();
            _toLog.Navigate();
        }

        private async Task UpdateCompaniesAsync()
        {
            (this as ILoading).StartLoading();
            CompaniesList = await _apiHelper.Companies.GetWhereTitleContains(_companyTitleToSearch);
            (this as ILoading).EndLoading();
        }
        #endregion

        #endregion

        public RegControlViewModel(APIHelper apiHelper, NavigationService<LogInControlViewModel> toLog)
        {
            Title = "Regestration";

            _apiHelper = apiHelper;
            _toLog = toLog;

            ToLogCommand = new RelayCommand(_ => _toLog.Navigate());
            NextCommand = new RelayCommand(OnNextCommandExecute, _ => 
                !string.IsNullOrWhiteSpace(Login) &&
                !string.IsNullOrWhiteSpace(Email) &&
                !string.IsNullOrWhiteSpace(Password) &&
                !string.IsNullOrWhiteSpace(ConfirmPassword));
            CompanySelectedCommand = new RelayCommand(OnCompanySelectedCommandExecute);
            NewCompanyCommand = new RelayCommand(OnNewCompanyCommandExecute);
            BackToCompaniesCommand = new RelayCommand(OnBackToCompaniesCommandExecute);
            CreateCompanyCommand = new RelayCommand(OnCreateCompanyCommandExecute, _ =>
                !string.IsNullOrWhiteSpace(NewCompanyTitle) &&
                !string.IsNullOrWhiteSpace(NewCompanyCode));
            BackToCompaniesFromCodeCommand = new RelayCommand(OnBackToCompaniesFromCodeCommandExecute);
            CompanyConfirmedCommand = new RelayCommand(OnCompanyConfirmedCommandExecute);
        }
    }
}
