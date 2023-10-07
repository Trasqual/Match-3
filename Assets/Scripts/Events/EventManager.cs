using System;
using System.Collections.Generic;
using Utilities;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class EventManager : Singleton<EventManager>
{
    private Dictionary<Type, List<Action<object>>> _eventsAndListeners = new();

    public void AddListener<T>(Action<object> listener)
    {
        if (_eventsAndListeners.TryGetValue(typeof(T), out var listeners))
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }
        else
        {
            listeners = new List<Action<object>> { listener };
            _eventsAndListeners.Add(typeof(T), listeners);
        }
    }

    public void RemoveListener<T>(Action<object> listener)
    {
        if (_eventsAndListeners.TryGetValue(typeof(T), out var listeners))
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }
    }

    public void TriggerEvent<T>(object data = null)
    {
        if (_eventsAndListeners.TryGetValue(typeof(T), out var listeners))
        {
            foreach (var listener in listeners)
            {
                listener?.Invoke(data);
            }
        }
    }
}
