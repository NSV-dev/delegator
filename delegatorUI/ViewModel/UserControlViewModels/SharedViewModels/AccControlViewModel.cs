using delegatorUI.Infrastructure.Commands.Base;
using delegatorUI.Infrastructure.Services;
using delegatorUI.Infrastructure.Stores;
using delegatorUI.Library.Api;
using delegatorUI.ViewModel.Base;
using System.Windows.Input;

namespace delegatorUI.ViewModel.UserControlViewModels.SharedViewModels
{
    public class AccControlViewModel : BaseViewModel
    {
        private readonly APIHelper _apiHelper;
        private readonly CompanyUserStore _companyUserStore;

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
                //error
                return;
            }

            _companyUserStore.CompanyUser.User.Password = StringCipher.Encrypt(ConfirmPassword, "delegator");
            await _apiHelper.Users.Update(_companyUserStore.CompanyUser.User);

            OldPassword = "";
            NewPassword = "";
            ConfirmPassword = "";
        }
        #endregion

        public AccControlViewModel(APIHelper apiHelper, CompanyUserStore companyUserStore)
        {
            _apiHelper = apiHelper;
            _companyUserStore = companyUserStore;

            ChangePasswordCommand = new RelayCommand(OnChangePasswordCommandExecute, _ =>
             !string.IsNullOrWhiteSpace(OldPassword) &&
             !string.IsNullOrWhiteSpace(NewPassword) &&
             !string.IsNullOrWhiteSpace(ConfirmPassword) &&
             NewPassword == ConfirmPassword);
        }
    }
}
