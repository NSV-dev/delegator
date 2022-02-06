using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delegatorUI.ViewModel.WindowViewModel
{
    public class WindowViewModel : BaseViewModel
    {        
        private readonly APIHelper _apiHelper;

        private string _title;
        public string Title 
        {
            get => _title;
            set => OnPropertyChanged(ref _title, value);
        }

        private List<User> _users;
        public List<User> Users
        {
            get => _users;
            set => OnPropertyChanged(ref _users, value);
        }

        public WindowViewModel(APIHelper apiHelper)
        {
            _apiHelper = apiHelper;

            Title = "123";
            Load();
        }

        private async void Load()
        {
            Users = await _apiHelper.GetAllUsers();
        }
    }
}
