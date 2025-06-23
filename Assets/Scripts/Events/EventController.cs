using System;

public class EventController<T,U,V>
{
    public event Action<T,U,V> baseEvent;
    public void InvokeEvent(T type1,U type2,V type3) => baseEvent?.Invoke(type1,type2,type3);
    public void AddListener(Action<T, U, V> listener) => baseEvent += listener;
    public void RemoveListener(Action<T, U, V> listener) => baseEvent -= listener;
}

public class EventController
{
    public event Action baseEvent;
    public void InvokeEvent() => baseEvent?.Invoke();
    public void AddListener(Action listener) => baseEvent += listener;
    public void RemoveListener(Action listener) => baseEvent -= listener;
}