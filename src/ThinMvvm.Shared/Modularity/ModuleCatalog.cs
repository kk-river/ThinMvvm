using Microsoft.Extensions.DependencyInjection;

namespace ThinMvvm.Modularity;

public class ModuleCatalog
{
    internal List<Func<IModule>> ModuleGenerators { get; } = [];

    public void AddModule<TModule>()
        where TModule : IModule, new()
    {
        ModuleGenerators.Add(static () => new TModule());
    }

    public void Build(IServiceCollection services)
    {
        foreach (IModule module in ModuleGenerators.Select(gen => gen()))
        {
            module.OnInitialize(services);
        }
    }
}
