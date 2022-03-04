using delegatorUI.Infrastructure.Services;
using delegatorUI.Infrastructure.Stores;
using delegatorUI.Library.Api;
using delegatorUI.Library.Models;
using delegatorUI.View.UserControls;
using delegatorUI.View.Window;
using delegatorUI.ViewModel.CreatingViewModels;
using delegatorUI.ViewModel.UserControlViewModels;
using delegatorUI.ViewModel.WindowViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace delegatorUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services
                .AddSingleton<APIHelper>()
                .AddSingleton<NavigationStore>()
                .AddSingleton<CompanyUserStore>()
                .AddSingleton<WindowViewModel>()
                .AddSingleton<MainWindow>(s => new() 
                    { DataContext = s.GetRequiredService<WindowViewModel>() })
                .AddTransient<LogInControlViewModel>()
                .AddTransient<LogInControl>(s => new() 
                    { DataContext = s.GetRequiredService<LogInControlViewModel>() })
                .AddTransient<RegControlViewModel>()
                .AddTransient<RegControl>(s => new()     
                    { DataContext = s.GetRequiredService<RegControlViewModel>() })
                .AddTransient<AdminControlViewModel>()
                .AddTransient<AdminControl>(s => new() 
                    { DataContext = s.GetRequiredService<AdminControlViewModel>() })
                .AddTransient<EmpControlViewModel>()
                .AddTransient<EmpControl>(s => new() 
                    { DataContext = s.GetRequiredService<EmpControlViewModel>() })
                ;

            services
                .AddSingleton(s => new NavigationService<RegControlViewModel>(
                    s.GetRequiredService<NavigationStore>(),
                    () => CreateViewModels.CreateRegViewModel(s)))
                .AddSingleton(s => new NavigationService<LogInControlViewModel>(
                    s.GetRequiredService<NavigationStore>(),
                    () => CreateViewModels.CreateLogInViewModel(s)));

            ConfigureEmpServices(services);
            ConfigureAdminServices(services);

            _serviceProvider = services.BuildServiceProvider();
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }
        
        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider.Dispose();

            base.OnExit(e);
        }

        private void ConfigureEmpServices(IServiceCollection services)
        {
            services
                .AddTransient<ViewModel.UserControlViewModels.EmpControlViewModels.TasksControlViewModel>()
                .AddTransient<View.UserControls.EmpControls.TasksControl>(s => new() 
                    { DataContext = s.GetRequiredService<ViewModel.UserControlViewModels.EmpControlViewModels.TasksControlViewModel>() })
                .AddTransient<ViewModel.UserControlViewModels.EmpControlViewModels.AccControlViewModel>()
                .AddTransient<View.UserControls.EmpControls.AccControl>(s => new() 
                    { DataContext = s.GetRequiredService<ViewModel.UserControlViewModels.EmpControlViewModels.AccControlViewModel>() });

            services
                .AddSingleton(s => new NavigationService<EmpControlViewModel>(
                    s.GetRequiredService<NavigationStore>(),
                    () => CreateViewModels.CreateEmpViewModel(s)));
        }

        private void ConfigureAdminServices(IServiceCollection services)
        {
            services
                .AddTransient<ViewModel.UserControlViewModels.AdminControlViewModels.AccControlViewModel>()
                .AddTransient<View.UserControls.AdminControls.AccControl>(s => new() 
                    { DataContext = s.GetRequiredService< ViewModel.UserControlViewModels.AdminControlViewModels.AccControlViewModel>() })
                .AddTransient<ViewModel.UserControlViewModels.AdminControlViewModels.TasksControlViewModel>()
                .AddTransient<View.UserControls.AdminControls.TasksControl>(s => new()
                    { DataContext = s.GetRequiredService<ViewModel.UserControlViewModels.AdminControlViewModels.TasksControlViewModel>() })
                .AddTransient<ViewModel.UserControlViewModels.AdminControlViewModels.CompanyControlViewModel>()
                .AddTransient<View.UserControls.AdminControls.CompanyControl>(s => new()
                    { DataContext = s.GetRequiredService<ViewModel.UserControlViewModels.AdminControlViewModels.CompanyControlViewModel>() });

            services
                .AddSingleton(s => new NavigationService<AdminControlViewModel>(
                    s.GetRequiredService<NavigationStore>(),
                    () => CreateViewModels.CreateAdminViewModel(s)));
        }
    }
}
