using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.ViewModel.UserControlViewModels.AdminControlViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace delegatorUI.ViewModel.CreatingViewModels.AdminViewModels
{
    public class CreateAdminViewModels
    {
        public static AccControlViewModel CreateAccViewModel(IServiceProvider serviceProvider, CompanyUser p)
            => new(
                serviceProvider.GetRequiredService<APIHelper>(),
                p);

        public static TasksControlViewModel CreateTasksViewModel(IServiceProvider serviceProvider, CompanyUser p)
            => new(
                serviceProvider.GetRequiredService<APIHelper>(),
                p);

        public static CompanyControlViewModel CreateCompanyViewModel(IServiceProvider serviceProvider, CompanyUser p)
            => new(
                serviceProvider.GetRequiredService<APIHelper>(),
                p);
    }
}
