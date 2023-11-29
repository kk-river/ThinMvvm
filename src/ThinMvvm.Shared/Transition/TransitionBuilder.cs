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
    private readonly Dictionary<string, Func<IServiceProvider, FrameworkElement>> _viewList = [];
    private readonly Dictionary<string, Func<IServiceProvider, Window>> _windowList = [];

    public ITransitionBuilder AddView<TView>(string viewName)
        where TView : FrameworkElement
    {
        _viewList.Add(viewName, provider => ActivatorUtilities.CreateInstance<TView>(provider));
        return this;
    }

    public ITransitionBuilder AddView<TView, TViewModel>(string viewName)
        where TView : FrameworkElement
        where TViewModel : class
    {
        _viewList.Add(viewName, provider =>
        {
            TView view = ActivatorUtilities.CreateInstance<TView>(provider);
            TViewModel viewModel = ActivatorUtilities.CreateInstance<TViewModel>(provider);
            view.DataContext = viewModel;
            return view;
        });

        return this;
    }

    public ITransitionBuilder AddView<TView>(string viewName, Func<IServiceProvider, TView> viewFactory)
        where TView : FrameworkElement
    {
        _viewList.Add(viewName, viewFactory);
        return this;
    }

    public ITransitionBuilder AddWindow<TWindow>(string windowName)
        where TWindow : Window
    {
        _windowList.Add(windowName, provider => ActivatorUtilities.CreateInstance<TWindow>(provider));
        return this;

    }

#if !WINUI
    public ITransitionBuilder AddWindow<TWindow, TViewModel>(string windowName)
        where TWindow : Window
        where TViewModel : class
    {
        _viewList.Add(windowName, provider =>
        {
            TWindow window = ActivatorUtilities.CreateInstance<TWindow>(provider);
            TViewModel viewModel = ActivatorUtilities.CreateInstance<TViewModel>(provider);
            window.DataContext = viewModel;
            return window;
        });

        return this;
    }
#endif

    public ITransitionBuilder AddWindow<TWindow>(string windowName, Func<IServiceProvider, TWindow> windowFactory)
        where TWindow : Window
    {
        _windowList.Add(windowName, windowFactory);
        return this;
    }

    internal void Build()
    {
        _services.AddSingleton<ITransitionManager>(provider => new TransitionManager(provider, _viewList, _windowList));
    }
}
