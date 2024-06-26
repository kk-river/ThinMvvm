using Microsoft.Extensions.DependencyInjection;
using ThinMvvm.Shared.Transition;

#if WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace ThinMvvm.Transition;

public class TransitionManager : ITransitionManager
{
    internal readonly Dictionary<string, FrameConfiguration> _frames = [];

    private readonly IServiceProvider _provider;

    public TransitionManager(IServiceProvider provider)
    {
        _provider = provider;
    }

    public void RequestTransition(string frameName, string viewName)
    {
        if (!_frames.TryGetValue(frameName, out FrameConfiguration? parent))
        {
            throw new KeyNotFoundException($"Frame '{frameName}' not found.");
        }

        ElementConfiguration<FrameworkElement> viewConf = _provider.GetRequiredKeyedService<ElementConfiguration<FrameworkElement>>(viewName);
        foreach ((string myFrameName, FrameConfiguration? myFrame) in viewConf.Frames)
        {
            _frames.TryAdd(myFrameName, myFrame);
        }

        FrameworkElement view = viewConf.CreateView(_provider);
        parent.PasteView(view);
    }

    public void ScheduleTransition(string frameName, string viewName)
    {
        ElementConfiguration<FrameworkElement> viewConf = _provider.GetRequiredKeyedService<ElementConfiguration<FrameworkElement>>(viewName);
        foreach ((string myFrameName, FrameConfiguration? myFrame) in viewConf.Frames)
        {
            _frames.TryAdd(myFrameName, myFrame);
            myFrame
        }

    }

    public void ShowWindow(string windowName)
    {
        ElementConfiguration<Window> conf = _provider.GetRequiredKeyedService<ElementConfiguration<Window>>(windowName);
        Window window = conf.CreateView(_provider);
        foreach ((string myFrameName, FrameConfiguration myFrame) in conf.Frames)
        {
            _frames.TryAdd(myFrameName, myFrame);
        }

#if WINUI
        window.Activate();
#else
        window.Show();
#endif
    }
}
