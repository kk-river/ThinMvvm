using Microsoft.Extensions.DependencyInjection;

#if WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace ThinMvvm.UIHelper;

public interface IViewHelperBuilder
{
    public IViewHelperBuilder AddView<TView>(object? key, Func<IServiceProvider, object?, TView> viewFactory, ServiceLifetime serviceLifetime)
        where TView : FrameworkElement;
}
