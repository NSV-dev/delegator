using delegatorUI.Infrastructure.Commands.Base;
using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace delegatorUI.ViewModel.WindowViewModel
{
    public class WindowViewModel : BaseViewModel
    {
        #region Title
        private string _title = "delegator";
        public string Title 
        {
            get => _title;
            set => OnPropertyChanged(ref _title, value);
        }
        #endregion

        #region Users
        private List<User> _users;
        public List<User> Users
        {
            get => _users;
            set => OnPropertyChanged(ref _users, value);
        }
        #endregion

        #region WindowState
        private WindowState _windowState;
        public WindowState WindowState
        {
            get => _windowState;
            set => OnPropertyChanged(ref _windowState, value);
        }

        public ICommand MinimazeCommand { get; set; }
        public ICommand MaximazeCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        #endregion

        public WindowViewModel()
        {
            MinimazeCommand = new RelayCommand(_ => WindowState = WindowState.Minimized);
            MaximazeCommand = new RelayCommand(_ => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized);
            CloseCommand = new RelayCommand(_ => Application.Current.Shutdown());
        }
    }
}
