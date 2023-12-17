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

    private static void OnFrameNameChanged(DependencyObject element, DependencyPropertyChangedEventArgs args) => s_frames.Add((string)args.NewValue, new Frame((FrameworkElement)element));

    public static void SetFrameName(DependencyObject regionTarget, string regionName) => regionTarget.SetValue(FrameNameProperty, regionName);

    public static string GetFrameName(DependencyObject regionTarget) => (string)regionTarget.GetValue(FrameNameProperty);
    #endregion Dependency

    internal static readonly Dictionary<string, IFrame> s_frames = [];
    public IReadOnlyDictionary<string, IFrame> Frames => s_frames;

    private readonly IServiceProvider _provider;

    internal TransitionManager(IServiceProvider provider)
    {
        _provider = provider;
    }

    public void RequestTransit(string frameName, string viewName)
    {
        if (!s_frames.TryGetValue(frameName, out IFrame? frame))
        {
            throw new KeyNotFoundException($"Frame '{frameName}' not found.");
        }

        ViewRegistration view = _provider.GetRequiredKeyedService<ViewRegistration>(viewName);
        ((Frame)frame).PasteView(view);
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
