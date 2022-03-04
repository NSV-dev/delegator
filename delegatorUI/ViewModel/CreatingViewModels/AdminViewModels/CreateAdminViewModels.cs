using delegatorUI.Infrastructure.Stores;
using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.UserControlViewModels.AdminControlViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace delegatorUI.ViewModel.CreatingViewModels.AdminViewModels
{
    public class CreateAdminViewModels
    {
        public static TasksControlViewModel CreateTasksViewModel(IServiceProvider serviceProvider)
            => new(
                serviceProvider.GetRequiredService<APIHelper>(),
                serviceProvider.GetRequiredService<CompanyUserStore>().CompanyUser);

        public static CompanyControlViewModel CreateCompanyViewModel(IServiceProvider serviceProvider)
            => new(
                serviceProvider.GetRequiredService<APIHelper>(),
                serviceProvider.GetRequiredService<CompanyUserStore>());
    }
}
