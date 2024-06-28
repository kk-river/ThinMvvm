using Microsoft.Extensions.DependencyInjection;

namespace ThinMvvm.Modularity;

public interface IModule
{
    void OnInitializing(IServiceCollection services);

    void OnInitialized(IServiceProvider provider);
}
