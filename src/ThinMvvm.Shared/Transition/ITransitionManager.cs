namespace ThinMvvm.Transition;

public interface ITransitionManager
{
    void RequestTransition(string frameName, string viewName);

    void ShowWindow(string viewName);
    //void ShowWindow(string viewName, string windowName);
}
