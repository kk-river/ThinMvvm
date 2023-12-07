namespace ThinMvvm.Transition;

public interface ITransitionManager
{
    IReadOnlyDictionary<string, IFrame> Frames { get; }

    void RequestTransit(string frameName, string viewName);

    void ShowWindow(string windowName);
}
