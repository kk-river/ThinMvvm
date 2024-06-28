#pragma warning disable CS0067

using System.Collections.Specialized;

namespace ThinMvvm.UIHelper;

public abstract class ViewModelBase : INotifyCollectionChanged
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;
}
