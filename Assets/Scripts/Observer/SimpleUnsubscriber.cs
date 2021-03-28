using System;
using System.Collections.Generic;

public class SimpleUnsubscriber<T> : IDisposable
{
    ICollection<IObserver<T>> allObserversRef;
    IObserver<T> myObserver;
    public SimpleUnsubscriber(ICollection<IObserver<T>> allObserversRef, IObserver<T> myObserver)
    {
        this.allObserversRef = allObserversRef;
        this.myObserver = myObserver;
        allObserversRef.Add(myObserver);
    }
    public void Dispose()
    {
        if (allObserversRef.Contains(myObserver))
        {
            allObserversRef.Remove(myObserver);
        }
    }
}
