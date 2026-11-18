using DataStructuresAssignment.PointsOfInterestManager.Implementation;
using DataStructuresAssignment.PointsOfInterestManager.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Windows;

namespace DataStructuresAssignment.PointOfInterestManagerUI;

public partial class App : Application
{
    private ServiceProvider Services { get; }

    public App()
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

        var services = new ServiceCollection();

        services.AddSingleton<IPointOfInterestService, PointOfInterestService>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainWindow>();

        Services = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        MainWindow = Services.GetRequiredService<MainWindow>();
        MainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Services.Dispose();
        base.OnExit(e);
    }
}