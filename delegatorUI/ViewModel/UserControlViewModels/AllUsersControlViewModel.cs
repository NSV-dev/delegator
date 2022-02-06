using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delegatorUI.ViewModel.UserControlViewModels
{
    public class AllUsersControlViewModel : BaseViewModel
    {
        private readonly APIHelper _apiHelper;

        #region Users
        private List<User> _users;
        public List<User> Users
        {
            get => _users;
            set => OnPropertyChanged(ref _users, value);
        }
        #endregion


        public AllUsersControlViewModel(APIHelper apiHelper)
        {
            _apiHelper = apiHelper;

            LoadUsersAsync();
        }

        private async void LoadUsersAsync()
        {
            Users = await _apiHelper.GetAllUsers();
        }
    }
}
