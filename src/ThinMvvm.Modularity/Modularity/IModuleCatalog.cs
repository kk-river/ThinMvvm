using Microsoft.Extensions.DependencyInjection;

namespace ThinMvvm.Modularity;

public interface IModuleCatalog
{
    void AddModule<TModule>()
        where TModule : IModule, new();

    void Build(IServiceCollection services);

    void RunPostBuild(IServiceProvider provider);
}
