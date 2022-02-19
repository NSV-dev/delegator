using delegatorUI.Infrastructure.Commands.Base;
using delegatorUI.Infrastructure.Interfaces;
using delegatorUI.Infrastructure.Services;
using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.Base;
using delegatorUI.ViewModel.UserControlViewModels.EmpControlViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace delegatorUI.ViewModel.UserControlViewModels
{
    public class EmpControlViewModel : BaseViewModel
    {
        private User _user;
        private Company _company;

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
            _user = companyUser.User;
            _company = companyUser.Company;

            ExitCommand = new RelayCommand(_ => exit.Navigate());
            ToTasksCommand = new RelayCommand(_ => CurrentViewModel = tasksControlViewModel());
            ToAccCommand = new RelayCommand(_ => CurrentViewModel = accControlViewModel());

            Title = _company.Title;
            ToTasksCommand.Execute(null);
        }
    }
}
