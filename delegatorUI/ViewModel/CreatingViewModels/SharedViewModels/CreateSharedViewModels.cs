using delegatorUI.Infrastructure.Stores;
using delegatorUI.Library.Api;
using delegatorUI.ViewModel.UserControlViewModels.SharedViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace delegatorUI.ViewModel.CreatingViewModels.SharedViewModels
{
    class CreateSharedViewModels
    {
        public static AccControlViewModel CreateAccViewModel(IServiceProvider serviceProvider)
            => new(
                serviceProvider.GetRequiredService<APIHelper>(),
                serviceProvider.GetRequiredService<CompanyUserStore>());

    }
}
