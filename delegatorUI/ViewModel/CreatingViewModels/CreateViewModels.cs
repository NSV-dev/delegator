﻿using delegatorUI.Infrastructure.Services;
using delegatorUI.Infrastructure.Stores;
using delegatorUI.Library.Api;
using delegatorUI.ViewModel.CreatingViewModels.AdminViewModels;
using delegatorUI.ViewModel.CreatingViewModels.EmpViewModels;
using delegatorUI.ViewModel.CreatingViewModels.SharedViewModels;
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
                serviceProvider.GetRequiredService<NavigationService<EmpControlViewModel>>(),
                serviceProvider.GetRequiredService<NavigationService<AdminControlViewModel>>(),
                serviceProvider.GetRequiredService<CompanyUserStore>());

        public static RegControlViewModel CreateRegViewModel(IServiceProvider serviceProvider)
            => new(
                serviceProvider.GetRequiredService<APIHelper>(),
                serviceProvider.GetRequiredService<NavigationService<LogInControlViewModel>>());

        public static AdminControlViewModel CreateAdminViewModel(IServiceProvider serviceProvider)
            => new(
                () => CreateSharedViewModels.CreateAccViewModel(serviceProvider),
                () => CreateAdminViewModels.CreateTasksViewModel(serviceProvider),
                () => CreateAdminViewModels.CreateCompanyViewModel(serviceProvider),
                serviceProvider.GetRequiredService<NavigationService<LogInControlViewModel>>(),
                serviceProvider.GetRequiredService<CompanyUserStore>());

        public static EmpControlViewModel CreateEmpViewModel(IServiceProvider serviceProvider)
            => new(
                () => CreateEmpViewModels.CreateTasksViewModel(serviceProvider),
                () => CreateSharedViewModels.CreateAccViewModel(serviceProvider),
                serviceProvider.GetRequiredService<NavigationService<LogInControlViewModel>>(),
                serviceProvider.GetRequiredService<CompanyUserStore>().CompanyUser);
    }
}
