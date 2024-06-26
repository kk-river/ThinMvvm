using Microsoft.Extensions.DependencyInjection;
using ThinMvvm.Shared.Transition;

#if WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace ThinMvvm.Transition;

public interface ITransitionBuilder
{
    public IViewConfiguration<TView> AddView<TView>(string viewName, Func<IServiceProvider, TView> viewFactory, ServiceLifetime serviceLifetime)
        where TView : FrameworkElement;

    public IWindowConfiguration<TWindow> AddWindow<TWindow>(string windowName, Func<IServiceProvider, TWindow> windowFactory)
        where TWindow : Window;
}
