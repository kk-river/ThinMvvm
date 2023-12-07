using Microsoft.Extensions.DependencyInjection;

#if WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace ThinMvvm.Transition;

public class TransitionManager : ITransitionManager
{
    #region Dependency
    public static readonly DependencyProperty FrameNameProperty = DependencyProperty.RegisterAttached("FrameName", typeof(string), typeof(TransitionManager), new PropertyMetadata(defaultValue: null, OnFrameNameChanged));

    private static void OnFrameNameChanged(DependencyObject element, DependencyPropertyChangedEventArgs args) => s_transitionFrames.Add((string)args.NewValue, new((FrameworkElement)element));

    public static void SetFrameName(DependencyObject regionTarget, string regionName) => regionTarget.SetValue(FrameNameProperty, regionName);

    public static string GetFrameName(DependencyObject regionTarget) => (string)regionTarget.GetValue(FrameNameProperty);
    #endregion Dependency

    internal static readonly Dictionary<string, Frame> s_transitionFrames = [];
    public IReadOnlyDictionary<string, IFrame> Frames => (IReadOnlyDictionary<string, IFrame>)s_transitionFrames;

    private readonly IServiceProvider _provider;

    internal TransitionManager(IServiceProvider provider)
    {
        _provider = provider;
    }

    public void RequestTransit(string frameName, string viewName)
    {
        if (!s_transitionFrames.TryGetValue(frameName, out Frame? frame))
        {
            throw new KeyNotFoundException($"Frame '{frameName}' not found.");
        }

        ViewRegistration view = _provider.GetRequiredKeyedService<ViewRegistration>(viewName);
        frame.PasteView(view);
    }

    public void ShowWindow(string windowName)
    {
        WindowRegistration window = _provider.GetRequiredKeyedService<WindowRegistration>(windowName);

#if WINUI
        window.Window.Activate();
#else
        window.Window.Show();
#endif
    }
}
