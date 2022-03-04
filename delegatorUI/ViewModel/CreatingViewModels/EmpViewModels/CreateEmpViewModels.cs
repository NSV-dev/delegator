using delegatorUI.Infrastructure.Stores;
using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.UserControlViewModels.EmpControlViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace delegatorUI.ViewModel.CreatingViewModels.EmpViewModels
{
    public class CreateEmpViewModels
    {
        public static TasksControlViewModel CreateTasksViewModel(IServiceProvider serviceProvider)
            => new(
                serviceProvider.GetRequiredService<APIHelper>(),
                serviceProvider.GetRequiredService<CompanyUserStore>().CompanyUser);

        public static AccControlViewModel CreateAccViewModel(IServiceProvider serviceProvider)
            => new(
                serviceProvider.GetRequiredService<APIHelper>(),
                serviceProvider.GetRequiredService<CompanyUserStore>());
    }
}
