#if WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace ThinMvvm.Transition;

internal record ViewRegistration(string ViewName, FrameworkElement ViewElement);

internal record WindowRegistration(string WindowName, Func<IServiceProvider, UIElement, Window> Factory);
