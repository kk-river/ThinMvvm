using Microsoft.Extensions.DependencyInjection;

namespace ThinMvvm.Modularity;

public class OnMemoryModuleCatalog : IModuleCatalog
{
    private readonly List<IModule> _modules = [];

    public void AddModule<TModule>()
        where TModule : IModule, new()
    {
        _modules.Add(new TModule());
    }

    public void Build(IServiceCollection services)
    {
        foreach (IModule module in _modules)
        {
            module.OnInitialize(services);
        }
    }
}
