#if WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace ThinMvvm.Transition;

public interface ITransitionBuilder
{
    public ITransitionBuilder AddView<TView>(string viewName)
        where TView : FrameworkElement;

    public ITransitionBuilder AddView<TView, TViewModel>(string viewName)
        where TView : FrameworkElement
        where TViewModel : class;

    public ITransitionBuilder AddView<TView>(string viewName, Func<IServiceProvider, TView> viewFactory)
        where TView : FrameworkElement;

    public ITransitionBuilder AddWindow<TWindow>(string viewName)
        where TWindow : Window;

#if !WINUI
    public ITransitionBuilder AddWindow<TWindow, TViewModel>(string viewName)
        where TWindow : Window
        where TViewModel : class;
#endif

    public ITransitionBuilder AddWindow<TWindow>(string viewName, Func<IServiceProvider, TWindow> windowFactory)
        where TWindow : Window;
}
