using Microsoft.Extensions.DependencyInjection;

namespace ThinMvvm.UIHelper;

public static class IServiceCollectionExtension
{
    public static IServiceCollection ConfigureViewHelper(this IServiceCollection services, Action<IViewHelperBuilder> configureHelper)
    {
        ViewHelperBuilder builder = new(services);
        configureHelper(builder);

        return services;
    }
}
