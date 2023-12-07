using ThinMvvm.Internal;

#if WINUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#else
using System.Windows;
using System.Windows.Controls;
#endif


namespace ThinMvvm.Transition;

public interface IFrame
{
    IObservable<string?> CurrentViewName { get; }
}

internal class Frame(FrameworkElement frameElement) : IFrame
{
    private readonly FrameworkElement _frameElement = frameElement;

    private readonly MinimumSubject<string?> _currentViewName = new();

    public IObservable<string?> CurrentViewName => _currentViewName;

    public void PasteView(ViewRegistration viewRegistration)
    {
        switch (_frameElement)
        {
            case ContentControl contentControl:
                contentControl.Content = viewRegistration.ViewElement;
                break;
            case ItemsControl itemsControl:
                itemsControl.Items.Add(viewRegistration.ViewElement);
                break;
            case Panel panel:
                panel.Children.Add(viewRegistration.ViewElement);
                break;
            default:
                throw new InvalidCastException($"{_frameElement.GetType().Name} must be assignable to one of {typeof(ContentControl).Name}, {typeof(ItemsControl).Name}, {typeof(Panel).Name}");
        }

        _currentViewName.OnNext(viewRegistration.ViewName);
    }
}
