using delegatorUI.Infrastructure.Stores;
using delegatorUI.Library.Api;
using delegatorUI.View.UserControls;
using delegatorUI.View.Window;
using delegatorUI.ViewModel.UserControlViewModels;
using delegatorUI.ViewModel.WindowViewModel;
using Microsoft.Extensions.DependencyInjection;
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
                .AddSingleton<WindowViewModel>()
                .AddSingleton<MainWindow>(s => new MainWindow() { DataContext = s.GetRequiredService<WindowViewModel>() })
                .AddTransient<LogInControlViewModel>()
                .AddTransient<LogInControl>(s => new LogInControl() { DataContext = s.GetRequiredService<LogInControlViewModel>() })
                .AddTransient<RegControlViewModel>()
                .AddTransient<RegControl>(s => new RegControl() { DataContext = s.GetRequiredService<RegControlViewModel>() })
                .AddTransient<AdminControlViewModel>()
                .AddTransient<AdminControl>(s => new AdminControl() { DataContext = s.GetRequiredService<AdminControlViewModel>() })
                .AddTransient<EmpControlViewModel>()
                .AddTransient<EmpControl>(s => new EmpControl() { DataContext = s.GetRequiredService<EmpControlViewModel>() })
                ;

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

    }
}
