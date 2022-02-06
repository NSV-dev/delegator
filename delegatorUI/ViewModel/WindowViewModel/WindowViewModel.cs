using delegatorUI.Infrastructure.Commands.Base;
using delegatorUI.Infrastructure.Stores;
using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using delegatorUI.ViewModel.UserControlViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace delegatorUI.ViewModel.WindowViewModel
{
    public class WindowViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

        #region Title
        private string _title;
        public string Title 
        {
            get => _title;
            set => OnPropertyChanged(ref _title, value);
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

        public WindowViewModel(NavigationStore navigationStore, LogInControlViewModel logInControlViewModel)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            MinimazeCommand = new RelayCommand(_ => WindowState = WindowState.Minimized);
            MaximazeCommand = new RelayCommand(_ => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized);
            CloseCommand = new RelayCommand(_ => Application.Current.Shutdown());

            _navigationStore.Title = "Logging In";
            _navigationStore.CurrentViewModel = logInControlViewModel;
        }

        private void OnCurrentViewModelChanged()
        {
            Title = "delegator | " + _navigationStore.Title;
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
