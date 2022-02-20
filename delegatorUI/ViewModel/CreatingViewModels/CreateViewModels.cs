using delegatorUI.Infrastructure.Services;
using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.CreatingViewModels.EmpViewModels;
using delegatorUI.ViewModel.CreatingViewModels.AdminViewModels;
using delegatorUI.ViewModel.UserControlViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace delegatorUI.ViewModel.CreatingViewModels
{
    public class CreateViewModels
    {
        public static LogInControlViewModel CreateLogInViewModel(IServiceProvider serviceProvider)
            => new(
                serviceProvider.GetRequiredService<APIHelper>(),
                serviceProvider.GetRequiredService<NavigationService<RegControlViewModel>>(),
                serviceProvider.GetRequiredService<ParameterNavigationService<CompanyUser, EmpControlViewModel>>(),
                serviceProvider.GetRequiredService<ParameterNavigationService<CompanyUser, AdminControlViewModel>>());

        public static RegControlViewModel CreateRegViewModel(IServiceProvider serviceProvider)
            => new(
                serviceProvider.GetRequiredService<APIHelper>(),
                serviceProvider.GetRequiredService<NavigationService<LogInControlViewModel>>());

        public static AdminControlViewModel CreateAdminViewModel(IServiceProvider serviceProvider, CompanyUser p)
            => new(
                () => CreateAdminViewModels.CreateAccViewModel(serviceProvider, p),
                () => CreateAdminViewModels.CreateTasksViewModel(serviceProvider, p),
                () => CreateAdminViewModels.CreateCompanyViewModel(serviceProvider, p),
                serviceProvider.GetRequiredService<NavigationService<LogInControlViewModel>>(),
                p);

        public static EmpControlViewModel CreateEmpViewModel(IServiceProvider serviceProvider, CompanyUser p)
            => new(
                () => CreateEmpViewModels.CreateTasksViewModel(serviceProvider, p),
                () => CreateEmpViewModels.CreateAccViewModel(serviceProvider, p),
                serviceProvider.GetRequiredService<NavigationService<LogInControlViewModel>>(),
                p);
    }
}
