
using System;

class KeyObserver<T, Key> : IObserver<T>
{
    private readonly IKeyObservable<T, Key> source;
    private readonly Action<T> onNext;
    private readonly IDisposable subscription;
    bool completed = false;

    public KeyObserver(IKeyObservable<T, Key> source, Key key, Action<T> onNext)
    {
        this.source = source;
        this.onNext = onNext;
        this.subscription = this.source.Subscribe(this, key);
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