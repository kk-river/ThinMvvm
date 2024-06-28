
#if WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace ThinMvvm.UIHelper;

public interface IViewHelper
{
    TView GetView<TView>(object? key)
        where TView : notnull;

    public TView CreateView<TView>();

    public TView CreateView<TView, TViewModel>()
        where TView : FrameworkElement;
}
