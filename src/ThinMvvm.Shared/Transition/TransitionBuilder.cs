using Microsoft.Extensions.DependencyInjection;

#if WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace ThinMvvm.Transition;

internal class TransitionBuilder(IServiceCollection services) : ITransitionBuilder
{
    private readonly IServiceCollection _services = services;

    public ITransitionBuilder AddView<TView>(string viewName, Func<IServiceProvider, TView> viewFactory, ServiceLifetime serviceLifetime)
        where TView : FrameworkElement
    {
        _services.Add(new ServiceDescriptor(typeof(ViewRegistration), viewName, (provider, _) => new ViewRegistration(viewName, viewFactory(provider)), serviceLifetime));

        return this;
    }

    public ITransitionBuilder AddWindow<TWindow>(string windowName, Func<IServiceProvider, TWindow> windowFactory, ServiceLifetime serviceLifetime)
        where TWindow : Window
    {
        _services.Add(new ServiceDescriptor(typeof(WindowRegistration), windowName, (provider, _) => new WindowRegistration(windowName, windowFactory(provider)), serviceLifetime));

        return this;
    }

    internal void Build()
    {
        _services.Add(new ServiceDescriptor(typeof(ITransitionManager), provider => new TransitionManager(provider), ServiceLifetime.Singleton));
    }
}
