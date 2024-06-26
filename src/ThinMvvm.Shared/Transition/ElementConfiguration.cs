using Microsoft.Extensions.DependencyInjection;
using ThinMvvm.Transition;

#if WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace ThinMvvm.Shared.Transition;

internal abstract class ElementConfiguration<TUIElement>(string name, Func<IServiceProvider, TUIElement> factory)
{
    public string Name { get; } = name;
    public abstract Type ViewType { get; }

    public Func<IServiceProvider, TUIElement> Factory { get; } = factory;

    private protected readonly Dictionary<string, FrameConfiguration> _frames = [];
    public IReadOnlyDictionary<string, FrameConfiguration> Frames => _frames;

    public abstract TUIElement CreateView(IServiceProvider provider);
}

internal abstract class ElementConfiguration<TUIElement, TView>(string name, Func<IServiceProvider, TUIElement> factory)
    : ElementConfiguration<TUIElement>(name, factory)
    where TUIElement : class
    where TView : class, TUIElement
{
    public override Type ViewType { get; } = typeof(TView);

    public override TUIElement CreateView(IServiceProvider provider)
    {
        return provider.GetRequiredKeyedService<TView>(Name);
    }

    public void RegisterFrame(string frameName, Action<TView, FrameworkElement> paster)
    {
        _frames.TryAdd(frameName, new FrameConfiguration<TView>(paster));
    }
}

internal class ViewConfiguration<TView>(string viewName, Func<IServiceProvider, TView> viewFactory)
    : ElementConfiguration<FrameworkElement, TView>(viewName, viewFactory), IViewConfiguration<TView>
    where TView : FrameworkElement;

internal class WindowConfiguration<TWindow>(string windowName, Func<IServiceProvider, TWindow> viewFactory)
    : ElementConfiguration<Window, TWindow>(windowName, viewFactory), IWindowConfiguration<TWindow>
    where TWindow : Window;
