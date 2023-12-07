using System.Collections.Immutable;
using System.Reflection;

namespace ThinMvvm.Internal;

internal class MinimumSubject<T> : IObservable<T>, IObserver<T>, IDisposable
{
    private static readonly ImmutableList<Subscription> s_terminated;
    private static readonly ImmutableList<Subscription> s_disposed;

    static MinimumSubject()
    {
        //Create different references for each static field to comparing.
        s_terminated = CreateImmutableList<Subscription>();
        s_disposed = CreateImmutableList<Subscription>();

        static ImmutableList<T1> CreateImmutableList<T1>()
        {
            Type type = typeof(ImmutableList<>).MakeGenericType(typeof(T1));

            ConstructorInfo constructor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, Type.EmptyTypes)!;
            return (ImmutableList<T1>)constructor.Invoke(null);
        }
    }

    private ImmutableList<Subscription> _subscriptions = ImmutableList<Subscription>.Empty;
    private Exception? _exception;

    public void OnNext(T value)
    {
        ImmutableList<Subscription> subscriptions = _subscriptions;

        if (_subscriptions == s_disposed)
        {
            _exception = null;
            ThrowDisposed();
            return;
        }

        foreach (Subscription subscription in subscriptions)
        {
            subscription.Observer?.OnNext(value);
        }
    }

    public void OnCompleted()
    {
        while (true)
        {
            ImmutableList<Subscription> subscriptions = _subscriptions;

            if (subscriptions == s_disposed)
            {
                _exception = null;
                ThrowDisposed();
                break;
            }

            if (subscriptions == s_terminated)
            {
                break;
            }

            if (Interlocked.CompareExchange(ref _subscriptions, s_terminated, subscriptions) == subscriptions)
            {
                foreach (Subscription subscription in subscriptions)
                {
                    subscription.Observer?.OnCompleted();
                }
                break;
            }
        }
    }

    public void OnError(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        while (true)
        {
            ImmutableList<Subscription> subscriptions = _subscriptions;

            if (subscriptions == s_disposed)
            {
                _exception = null;
                ThrowDisposed();
                break;
            }

            if (subscriptions == s_terminated)
            {
                break;
            }

            _exception = exception;

            if (Interlocked.CompareExchange(ref _subscriptions, s_terminated, subscriptions) == subscriptions)
            {
                foreach (Subscription subscription in subscriptions)
                {
                    subscription.Observer?.OnError(exception);
                }
                break;
            }
        }
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        Subscription subscription = new(this, observer);
        while (true)
        {
            ImmutableList<Subscription> subscriptions = _subscriptions;

            if (subscriptions == s_disposed)
            {
                _exception = null;
                ThrowDisposed();
                break;
            }

            if (subscriptions == s_terminated)
            {
                if (_exception is not Exception exception)
                {
                    observer.OnCompleted();
                }
                else
                {
                    observer.OnError(exception);
                }

                break;
            }

            ImmutableList<Subscription> newSubscriptions = subscriptions.Add(subscription);

            while (Interlocked.CompareExchange(ref _subscriptions, newSubscriptions, subscriptions) == subscriptions)
            {
                return subscription;
            }
        }

        return EmptyDisposable.Default;
    }

    private void Unsubscribe(Subscription subscription)
    {
        while (_subscriptions is ImmutableList<Subscription> before)
        {
            int targetIndex = before.IndexOf(subscription);
            if (targetIndex is -1) { return; }

            ImmutableList<Subscription> after = before.RemoveAt(targetIndex);

            while (Interlocked.CompareExchange(ref _subscriptions, after, before) == before)
            {
                return;
            }
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Interlocked.Exchange(ref _subscriptions, s_disposed);
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private static void ThrowDisposed() => throw new ObjectDisposedException(string.Empty);


    private class Subscription(MinimumSubject<T> subject, IObserver<T> observer) : IDisposable
    {
        private MinimumSubject<T> _subject = subject;
        private IObserver<T>? _observer = observer;

        public IObserver<T>? Observer => _observer;

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _observer, null) is null)
            {
                return;
            }

            _subject.Unsubscribe(this);
            _subject = null!;
        }
    }

    private class EmptyDisposable : IDisposable
    {
        public static readonly EmptyDisposable Default = new();

        public void Dispose() { }
    }
}
