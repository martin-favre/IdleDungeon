
using System;

class KeyObserver<Key, T> : IObserver<T>
{
    private readonly IKeyObservable<Key, T> source;
    private readonly Action<T> onNext;
    private readonly IDisposable subscription;
    bool completed = false;

    public KeyObserver(IKeyObservable<Key, T> source, Key key, Action<T> onNext)
    {
        this.source = source;
        this.onNext = onNext;
        this.subscription = this.source.Subscribe(key, this);
    }

    ~KeyObserver()
    {
        if (!completed)
        {
            this.subscription.Dispose();
        }
    }

    public void OnCompleted()
    {
        completed = true;
        this.subscription.Dispose();
    }

    public void OnError(Exception error)
    {
        throw error;
    }

    public void OnNext(T value)
    {
        onNext(value);
    }
}