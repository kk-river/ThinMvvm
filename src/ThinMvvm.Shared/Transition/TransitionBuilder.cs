using Microsoft.Extensions.DependencyInjection;
using ThinMvvm.Shared.Transition;

#if WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace ThinMvvm.Transition;

internal class TransitionBuilder(IServiceCollection services) : ITransitionBuilder
{
    private readonly IServiceCollection _services = services;

    public IViewConfiguration<TView> AddView<TView>(string viewName, Func<IServiceProvider, TView> viewFactory, ServiceLifetime serviceLifetime)
        where TView : FrameworkElement
    {
        ViewConfiguration<TView> viewConfiguration = new(viewName, viewFactory);

        _services.Add(new ServiceDescriptor(typeof(TView), viewName, (provider, _) => viewFactory(provider), serviceLifetime));
        _services.Add(new ServiceDescriptor(typeof(ElementConfiguration<FrameworkElement>), viewName, viewConfiguration));

        return viewConfiguration;
    }

    public IWindowConfiguration<TWindow> AddWindow<TWindow>(string windowName, Func<IServiceProvider, TWindow> windowFactory)
        where TWindow : Window
    {
        WindowConfiguration<TWindow> windowConfiguration = new(windowName, windowFactory);

        _services.Add(new ServiceDescriptor(typeof(TWindow), windowName, (provider, _) => windowFactory(provider), ServiceLifetime.Transient));
        _services.Add(new ServiceDescriptor(typeof(ElementConfiguration<Window>), windowName, windowConfiguration));

        return windowConfiguration;
    }
}
