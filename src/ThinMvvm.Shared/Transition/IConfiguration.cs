#if WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace ThinMvvm.Shared.Transition;

public interface IConfiguration<T>
    where T : class
{
    void RegisterFrame(string frameName, Action<T, FrameworkElement> paster);
}

public interface IViewConfiguration<TView> : IConfiguration<TView>
    where TView : FrameworkElement
{
}

public interface IWindowConfiguration<TWindow> : IConfiguration<TWindow>
    where TWindow : Window
{
}
