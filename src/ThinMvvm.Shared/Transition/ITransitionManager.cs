namespace ThinMvvm.Transition;

public interface ITransitionManager
{
    void RequestTransit(string frameName, string viewName);

    void ShowWindow(string windowName);
}
