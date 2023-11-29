using ThinMvvm.Modularity;
using Microsoft.Extensions.DependencyInjection;

#if WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace ThinMvvm;

public abstract class ThinApplication : Application
{
#if WINUI
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
#else
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
#endif

        IServiceCollection services = new ServiceCollection();

        ConfigureServices(services);

        ModuleCatalog moduleCatalog = new();
        ConfigureModuleCatalog(moduleCatalog);
        moduleCatalog.Build(services);

        PostStartup(provider: services.BuildServiceProvider());
    }

    protected virtual void ConfigureModuleCatalog(ModuleCatalog moduleCatalog) { }

    protected virtual void ConfigureServices(IServiceCollection services) { }

    protected virtual void PostStartup(IServiceProvider provider) { }
}
