#if WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace ThinMvvm.Transition;

public abstract class FrameConfiguration
{
    private protected readonly List<Func<FrameworkElement>> _schedules = [];

    public abstract void PasteView(FrameworkElement element);
    public abstract void ScheduleView(Func<FrameworkElement> elementFunc);

    public void ExecuteSchedules()
    {
        foreach (Func<FrameworkElement> schedule in _schedules)
        {
            PasteView(schedule());
        }
    }
}

internal class FrameConfiguration<TView>(Action<TView, FrameworkElement> paster) : FrameConfiguration
    where TView : class
{
    private readonly Action<TView, FrameworkElement> _paster = paster;

    public override void PasteView(FrameworkElement element)
    {
        if (_view is not null && _view.TryGetTarget(out TView? frame))
        {
            _paster(frame, element);
        }
    }

    public override void ScheduleView(Func<FrameworkElement> elementFunc)
    {
        _schedules.Add(elementFunc);
    }
}
