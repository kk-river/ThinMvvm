using Microsoft.Extensions.DependencyInjection;

#if WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace ThinMvvm.Transition;

public interface ITransitionBuilder
{
    public ITransitionBuilder AddView<TView>(string viewName, Func<IServiceProvider, TView> viewFactory, ServiceLifetime serviceLifetime)
        where TView : FrameworkElement;

    public ITransitionBuilder AddWindow<TWindow>(string viewName, Func<IServiceProvider, TWindow> windowFactory, ServiceLifetime serviceLifetime)
        where TWindow : Window;
}
