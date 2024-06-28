using Microsoft.Extensions.DependencyInjection;

#if WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace ThinMvvm.UIHelper;

internal class ViewHelperBuilder(IServiceCollection services) : IViewHelperBuilder
{
    private readonly IServiceCollection _services = services;

    public IViewHelperBuilder AddView<TView>(object? key, Func<IServiceProvider, object?, TView> viewFactory, ServiceLifetime serviceLifetime)
        where TView : FrameworkElement
    {
        _services.Add(new ServiceDescriptor(typeof(TView), key, viewFactory, serviceLifetime));

        return this;
    }
}
