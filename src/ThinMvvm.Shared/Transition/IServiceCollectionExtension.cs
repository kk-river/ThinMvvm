using Microsoft.Extensions.DependencyInjection;

namespace ThinMvvm.Transition;

public static class IServiceCollectionExtension
{
    public static IServiceCollection ConfigureTransition(this IServiceCollection services, Action<ITransitionBuilder> configureTransition)
    {
        TransitionBuilder builder = new(services);
        configureTransition(builder);

        return services;
    }
}
