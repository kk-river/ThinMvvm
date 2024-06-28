using Microsoft.Extensions.DependencyInjection;

#if WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace ThinMvvm.UIHelper;

public static class IViewHelperBuilderExtensions
{
    public static IViewHelperBuilder AddTransientView<TView>(this IViewHelperBuilder builder, object? key)
        where TView : FrameworkElement
        => builder.AddTransientView<TView, TView>(key);

    public static IViewHelperBuilder AddTransientView<TView, TViewImpl>(this IViewHelperBuilder builder, object? key)
        where TView : FrameworkElement
        where TViewImpl : TView
    {
        return builder.AddView(key, static (provider, _) => ActivatorUtilities.CreateInstance<TView>(provider), ServiceLifetime.Transient);
    }

    public static IViewHelperBuilder AddTransientViewWithViewModel<TView, TViewModel>(this IViewHelperBuilder builder, object? key)
        where TView : FrameworkElement
        where TViewModel : class
        => builder.AddTransientViewWithViewModel<TView, TView, TViewModel>(key);

    public static IViewHelperBuilder AddTransientViewWithViewModel<TView, TViewImpl, TViewModelImpl>(this IViewHelperBuilder builder, object? key)
        where TView : FrameworkElement
        where TViewImpl : TView
        where TViewModelImpl : class
    {
        return builder.AddView(key, static (provider, _) =>
        {
            TView view = ActivatorUtilities.CreateInstance<TViewImpl>(provider);
            TViewModelImpl viewModel = ActivatorUtilities.CreateInstance<TViewModelImpl>(provider);
            view.DataContext = viewModel;
            return view;
        }, ServiceLifetime.Transient);
    }

    public static IViewHelperBuilder AddTransientView<TView>(this IViewHelperBuilder builder, object? key, Func<IServiceProvider, object?, TView> viewFactory)
        where TView : FrameworkElement
    {
        return builder.AddView(key, viewFactory, ServiceLifetime.Transient);
    }


    // Singleton

    public static IViewHelperBuilder AddSingletonView<TView>(this IViewHelperBuilder builder, object? key)
        where TView : FrameworkElement
        => builder.AddSingletonView<TView, TView>(key);

    public static IViewHelperBuilder AddSingletonView<TView, TViewImpl>(this IViewHelperBuilder builder, object? key)
        where TView : FrameworkElement
        where TViewImpl : TView
    {
        return builder.AddView<TView>(key, static (provider, _) => ActivatorUtilities.CreateInstance<TViewImpl>(provider), ServiceLifetime.Singleton);
    }

    public static IViewHelperBuilder AddSingletonViewWithViewModel<TView, TViewModel>(this IViewHelperBuilder builder, object? key)
        where TView : FrameworkElement
        where TViewModel : class
        => builder.AddSingletonViewWithViewModel<TView, TView, TViewModel>(key);

    public static IViewHelperBuilder AddSingletonViewWithViewModel<TView, TViewImpl, TViewModel>(this IViewHelperBuilder builder, object? key)
        where TView : FrameworkElement
        where TViewImpl : TView
        where TViewModel : class
    {
        return builder.AddView(key, static (provider, _) =>
        {
            TView view = ActivatorUtilities.CreateInstance<TViewImpl>(provider);
            TViewModel viewModel = ActivatorUtilities.CreateInstance<TViewModel>(provider);
            view.DataContext = viewModel;
            return view;
        }, ServiceLifetime.Singleton);
    }

    public static IViewHelperBuilder AddSingletonView<TView>(this IViewHelperBuilder builder, object? key, Func<IServiceProvider, object?, TView> viewFactory)
        where TView : FrameworkElement
    {
        return builder.AddView(key, viewFactory, ServiceLifetime.Singleton);
    }
}
