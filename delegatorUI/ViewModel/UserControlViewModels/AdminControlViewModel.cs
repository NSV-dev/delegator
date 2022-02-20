using delegatorUI.Infrastructure.Commands.Base;
using delegatorUI.Infrastructure.Services;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using delegatorUI.ViewModel.UserControlViewModels.AdminControlViewModels;
using System;
using System.Windows.Input;

namespace delegatorUI.ViewModel.UserControlViewModels
{
    public class AdminControlViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set => OnPropertyChanged(ref _currentViewModel, value);
        }

        #region Commands
        public ICommand ExitCommand { get; }
        public ICommand ToTasksCommand { get; }
        public ICommand ToAccCommand { get; }
        public ICommand ToCompanyCommand { get; }
        #endregion

        public AdminControlViewModel(
            Func<AccControlViewModel> createAccControlViewModel,
            Func<TasksControlViewModel> createTasksControlViewModel,
            Func<CompanyControlViewModel> createCompanyControlViewModel,
            NavigationService<LogInControlViewModel> exit,
            CompanyUser companyUser)
        {
            Title = companyUser.Company.Title;

            ExitCommand = new RelayCommand(_ => exit.Navigate());
            ToAccCommand = new RelayCommand(_ => CurrentViewModel = createAccControlViewModel());
            ToTasksCommand = new RelayCommand(_ => CurrentViewModel = createTasksControlViewModel());
            ToCompanyCommand = new RelayCommand(_ => CurrentViewModel = createCompanyControlViewModel());

            ToTasksCommand.Execute(null);
        }
    }
}
