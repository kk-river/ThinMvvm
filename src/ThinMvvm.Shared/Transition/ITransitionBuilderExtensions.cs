using Microsoft.Extensions.DependencyInjection;
using ThinMvvm.Shared.Transition;

#if WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace ThinMvvm.Transition;

public static class ITransitionBuilderExtensions
{
    #region AddView
    public static IViewConfiguration<TView> AddTransientView<TView>(this ITransitionBuilder builder, string viewName)
        where TView : FrameworkElement
    {
        return builder.AddView(viewName, provider => ActivatorUtilities.CreateInstance<TView>(provider), ServiceLifetime.Transient);
    }

    public static IViewConfiguration<TView> AddTransientView<TView, TViewModel>(this ITransitionBuilder builder, string viewName)
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

    public static IViewConfiguration<TView> AddTransientView<TView>(this ITransitionBuilder builder, string viewName, Func<IServiceProvider, TView> viewFactory)
        where TView : FrameworkElement
    {
        return builder.AddView(viewName, viewFactory, ServiceLifetime.Transient);
    }

    public static IViewConfiguration<TView> AddSingletonView<TView>(this ITransitionBuilder builder, string viewName)
        where TView : FrameworkElement
    {
        return builder.AddView(viewName, provider => ActivatorUtilities.CreateInstance<TView>(provider), ServiceLifetime.Singleton);
    }

    public static IViewConfiguration<TView> AddSingletonView<TView, TViewModel>(this ITransitionBuilder builder, string viewName)
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

    public static IViewConfiguration<TView> AddSingletonView<TView>(this ITransitionBuilder builder, string viewName, Func<IServiceProvider, TView> viewFactory)
        where TView : FrameworkElement
    {
        return builder.AddView(viewName, viewFactory, ServiceLifetime.Singleton);
    }
    #endregion AddView


    #region AddWindow
    public static IWindowConfiguration<TWindow> AddWindow<TWindow>(this ITransitionBuilder builder, string windowName)
        where TWindow : Window
    {
        return builder.AddWindow(windowName, provider => ActivatorUtilities.CreateInstance<TWindow>(provider));
    }

#if !WINUI
    public static IWindowConfiguration<TWindow> AddWindow<TWindow, TViewModel>(this ITransitionBuilder builder, string windowName)
        where TWindow : Window
        where TViewModel : class
    {
        return builder.AddWindow(windowName, provider =>
        {
            TWindow window = ActivatorUtilities.CreateInstance<TWindow>(provider);
            TViewModel viewModel = ActivatorUtilities.CreateInstance<TViewModel>(provider);

            window.DataContext = viewModel;

            return window;
        });
    }
#endif
    #endregion AddWindow
}
