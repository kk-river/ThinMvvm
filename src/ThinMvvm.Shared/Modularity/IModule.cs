using Microsoft.Extensions.DependencyInjection;

namespace ThinMvvm.Modularity;

public interface IModule
{
    void OnInitialize(IServiceCollection services);
}
