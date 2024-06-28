using Microsoft.Extensions.DependencyInjection;

namespace ThinMvvm.Modularity;

public class ModuleCatalog : IModuleCatalog
{
    private protected readonly List<IModule> _modules = [];

    public void AddModule<TModule>()
        where TModule : IModule, new()
    {
        _modules.Add(new TModule());
    }

    public virtual void Build(IServiceCollection services)
    {
        foreach (IModule module in _modules)
        {
            module.OnInitializing(services);
        }
    }

    public virtual void RunPostBuild(IServiceProvider provider)
    {
        foreach (IModule module in _modules)
        {
            module.OnInitialized(provider);
        }
    }
}
