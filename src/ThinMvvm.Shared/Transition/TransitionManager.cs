using System.ComponentModel;

#if WINUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#else
using System.Windows;
using System.Windows.Controls;
#endif

namespace ThinMvvm.Transition;

public class TransitionManager : ITransitionManager
{
    #region Dependency
    public static readonly DependencyProperty FrameNameProperty = DependencyProperty.RegisterAttached("FrameName", typeof(string), typeof(TransitionManager), new PropertyMetadata(defaultValue: null, OnFrameNameChanged));

    private static void OnFrameNameChanged(DependencyObject element, DependencyPropertyChangedEventArgs args) => TransitionFrames.Add((string)args.NewValue, (FrameworkElement)element);

    public static void SetFrameName(DependencyObject regionTarget, string regionName) => regionTarget.SetValue(FrameNameProperty, regionName);

    public static string GetFrameName(DependencyObject regionTarget) => (string)regionTarget.GetValue(FrameNameProperty);
    #endregion Dependency

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static readonly Dictionary<string, object> TransitionFrames = [];

    private readonly IServiceProvider _provider;
    private readonly Dictionary<string, Func<IServiceProvider, FrameworkElement>> _views;
    private readonly Dictionary<string, Func<IServiceProvider, Window>> _windows;

    internal TransitionManager(IServiceProvider provider, Dictionary<string, Func<IServiceProvider, FrameworkElement>> views, Dictionary<string, Func<IServiceProvider, Window>> windows)
    {
        _provider = provider;
        _windows = windows;
        _views = views;
    }

    public void RequestTransit(string frameName, string viewName)
    {
        if (!TransitionFrames.TryGetValue(frameName, out object? frame))
        {
            throw new KeyNotFoundException($"Frame '{frameName}' not found.");
        }

        if (!_views.TryGetValue(viewName, out Func<IServiceProvider, FrameworkElement>? viewCreator))
        {
            throw new InvalidOperationException($"No view registered with name '{viewName}'.");
        }

        FrameworkElement view = viewCreator(_provider);
        switch (frame)
        {
            case ContentControl contentControl:
                contentControl.Content = view;
                break;
            case ItemsControl itemsControl:
                itemsControl.Items.Add(view);
                break;
            case Panel panel:
                panel.Children.Add(view);
                break;
            default:
                throw new InvalidCastException($"{frame.GetType().Name} must be assignable to one of {typeof(ContentControl).Name}, {typeof(ItemsControl).Name}, {typeof(Panel).Name}");
        }
    }

    public void ShowWindow(string windowName)
    {
        if (!_windows.TryGetValue(windowName, out Func<IServiceProvider, Window>? windowCreator))
        {
            throw new InvalidOperationException($"No window registered with name '{windowName}'.");
        }

        Window window = windowCreator(_provider);

#if WINUI
        window.Activate();
#else
        window.Show();
#endif

    }
}
