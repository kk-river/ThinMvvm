using Microsoft.Extensions.DependencyInjection;

#if WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace ThinMvvm.Transition;

public static class ITransitionBuilderExtensions
{
    #region AddView
    public static ITransitionBuilder AddTransientView<TView>(this ITransitionBuilder builder, string viewName)
        where TView : FrameworkElement
    {
        return builder.AddView(viewName, provider => ActivatorUtilities.CreateInstance<TView>(provider), ServiceLifetime.Transient);
    }

    public static ITransitionBuilder AddTransientView<TView, TViewModel>(this ITransitionBuilder builder, string viewName)
        where TView : FrameworkElement
        where TViewModel : class
    {
        return builder.AddView(viewName, provider =>
        {
            TView view = ActivatorUtilities.CreateInstance<TView>(provider);
            TViewModel viewModel = ActivatorUtilities.CreateInstance<TViewModel>(provider);
            view.DataContext = viewModel;
            return view;
        }, ServiceLifetime.Transient);
    }

    public static ITransitionBuilder AddTransientView<TView, TViewModel>(this ITransitionBuilder builder, string viewName, Func<IServiceProvider, TView> viewFactory)
        where TView : FrameworkElement
        where TViewModel : class
    {
        return builder.AddView(viewName, viewFactory, ServiceLifetime.Transient);
    }

    public static ITransitionBuilder AddSingletonView<TView>(this ITransitionBuilder builder, string viewName)
        where TView : FrameworkElement
    {
        return builder.AddView(viewName, provider => ActivatorUtilities.CreateInstance<TView>(provider), ServiceLifetime.Singleton);
    }

    public static ITransitionBuilder AddSingletonView<TView, TViewModel>(this ITransitionBuilder builder, string viewName)
        where TView : FrameworkElement
        where TViewModel : class
    {
        return builder.AddView(viewName, provider =>
        {
            TView view = ActivatorUtilities.CreateInstance<TView>(provider);
            TViewModel viewModel = ActivatorUtilities.CreateInstance<TViewModel>(provider);
            view.DataContext = viewModel;
            return view;
        }, ServiceLifetime.Singleton);
    }

    public static ITransitionBuilder AddSingletonView<TView, TViewModel>(this ITransitionBuilder builder, string viewName, Func<IServiceProvider, TView> viewFactory)
        where TView : FrameworkElement
        where TViewModel : class
    {
        return builder.AddView(viewName, viewFactory, ServiceLifetime.Singleton);
    }
    #endregion AddView


    #region AddWindow
    public static ITransitionBuilder AddTransientWindow<TWindow>(this ITransitionBuilder builder, string windowName)
        where TWindow : Window
    {
        return builder.AddWindow(windowName, provider => ActivatorUtilities.CreateInstance<TWindow>(provider), ServiceLifetime.Transient);
    }

#if !WINUI
    public static ITransitionBuilder AddTransientWindow<TWindow, TViewModel>(this ITransitionBuilder builder, string windowName)
        where TWindow : Window
        where TViewModel : class
    {
        return builder.AddWindow(windowName, provider =>
        {
            TWindow window = ActivatorUtilities.CreateInstance<TWindow>(provider);
            TViewModel viewModel = ActivatorUtilities.CreateInstance<TViewModel>(provider);
            window.DataContext = viewModel;
            return window;
        }, ServiceLifetime.Transient);
    }
#endif

    public static ITransitionBuilder AddTransientWindow<TWindow>(this ITransitionBuilder builder, string windowName, Func<IServiceProvider, TWindow> windowFactory, ServiceLifetime serviceLifetime)
        where TWindow : Window
    {
        return builder.AddWindow(windowName, windowFactory, ServiceLifetime.Transient);
    }

    public static ITransitionBuilder AddSingletonWindow<TWindow>(this ITransitionBuilder builder, string windowName)
        where TWindow : Window
    {
        return builder.AddWindow(windowName, provider => ActivatorUtilities.CreateInstance<TWindow>(provider), ServiceLifetime.Singleton);
    }

#if !WINUI
    public static ITransitionBuilder AddSingletonWindow<TWindow, TViewModel>(this ITransitionBuilder builder, string windowName)
        where TWindow : Window
        where TViewModel : class
    {
        return builder.AddWindow(windowName, provider =>
        {
            TWindow window = ActivatorUtilities.CreateInstance<TWindow>(provider);
            TViewModel viewModel = ActivatorUtilities.CreateInstance<TViewModel>(provider);
            window.DataContext = viewModel;
            return window;
        }, ServiceLifetime.Singleton);
    }
#endif

    public static ITransitionBuilder AddSingletonWindow<TWindow>(this ITransitionBuilder builder, string windowName, Func<IServiceProvider, TWindow> windowFactory, ServiceLifetime serviceLifetime)
        where TWindow : Window
    {
        return builder.AddWindow(windowName, windowFactory, ServiceLifetime.Singleton);
    }
    #endregion AddWindow
}
