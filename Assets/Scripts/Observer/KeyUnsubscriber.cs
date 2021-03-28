using System;
using System.Collections.Generic;

public class KeyUnsubscriber<T, Key> : IDisposable
{
    private readonly Dictionary<Key, List<IObserver<T>>> allObserversRef;
    private readonly IObserver<T> myObserver;
    private readonly Key key;
    public KeyUnsubscriber(Dictionary<Key, List<IObserver<T>>> allObserversRef, Key key, IObserver<T> myObserver)
    {
        this.myObserver = myObserver;
        this.allObserversRef = allObserversRef;
        this.key = key;
        List<IObserver<T>> obs;
        bool success = allObserversRef.TryGetValue(key, out obs);
        if (!success)
        {
            obs = new List<IObserver<T>>();
            obs.Add(myObserver);
            allObserversRef[key] = obs;
        }
        else
        {
            allObserversRef[key].Add(myObserver);
        }
    }
    public void Dispose()
    {
        List<IObserver<T>> obs;
        bool success = allObserversRef.TryGetValue(key, out obs);

        if (success)
        {
            obs.Remove(myObserver);
        }
    }
}
