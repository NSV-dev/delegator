using delegatorUI.Infrastructure.Commands.Base;
using delegatorUI.Infrastructure.Services;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using delegatorUI.ViewModel.UserControlViewModels.EmpControlViewModels;
using System;
using System.Windows.Input;

namespace delegatorUI.ViewModel.UserControlViewModels
{
    public class EmpControlViewModel : BaseViewModel
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
        #endregion

        public EmpControlViewModel(
            Func<TasksControlViewModel> tasksControlViewModel,
            Func<AccControlViewModel> accControlViewModel,
            NavigationService<LogInControlViewModel> exit,
            CompanyUser companyUser)
        {
            ExitCommand = new RelayCommand(_ => exit.Navigate());
            ToTasksCommand = new RelayCommand(_ => CurrentViewModel = tasksControlViewModel());
            ToAccCommand = new RelayCommand(_ => CurrentViewModel = accControlViewModel());

            Title = companyUser.Company.Title;
            ToTasksCommand.Execute(null);
        }
    }
}
