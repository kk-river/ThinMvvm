using Microsoft.Extensions.DependencyInjection;

#if WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace ThinMvvm.UIHelper;

public class ViewHelper(IServiceProvider provider) : IViewHelper
{
    private readonly IServiceProvider _provider = provider;

    public TView GetView<TView>(object? key)
        where TView : notnull
        => _provider.GetRequiredKeyedService<TView>(key);

    public TView CreateView<TView>()
        => ActivatorUtilities.CreateInstance<TView>(_provider);

    public TView CreateView<TView, TViewModel>()
        where TView : FrameworkElement
    {
        TView view = ActivatorUtilities.CreateInstance<TView>(_provider);
        view.DataContext = ActivatorUtilities.CreateInstance<TViewModel>(_provider);

        return view;
    }
}
