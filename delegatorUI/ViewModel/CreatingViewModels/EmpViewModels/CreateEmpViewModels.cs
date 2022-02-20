using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.UserControlViewModels.EmpControlViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace delegatorUI.ViewModel.CreatingViewModels.EmpViewModels
{
    public class CreateEmpViewModels
    {
        public static TasksControlViewModel CreateTasksViewModel(IServiceProvider serviceProvider, CompanyUser p)
            => new(
                serviceProvider.GetRequiredService<APIHelper>(),
                p);

        public static AccControlViewModel CreateAccViewModel(IServiceProvider serviceProvider, CompanyUser p)
            => new(
                serviceProvider.GetRequiredService<APIHelper>(),
                p);
    }
}
