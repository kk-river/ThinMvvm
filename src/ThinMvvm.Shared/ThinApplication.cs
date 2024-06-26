using ThinMvvm.Modularity;
using Microsoft.Extensions.DependencyInjection;
using ThinMvvm.Transition;


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

        services.AddSingleton<ITransitionManager, TransitionManager>();

        IModuleCatalog moduleCatalog = CreateModuleCatalog();
        ConfigureModuleCatalog(moduleCatalog);
        moduleCatalog.Build(services);

        ServiceProvider provider = services.BuildServiceProvider();
        PostStartup(provider);

        moduleCatalog.RunPostBuild(provider);
    }

    protected virtual IModuleCatalog CreateModuleCatalog() => new OnMemoryModuleCatalog();

    protected virtual void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) { }

    protected virtual void ConfigureServices(IServiceCollection services) { }

    protected virtual void PostStartup(IServiceProvider provider) { }
}
