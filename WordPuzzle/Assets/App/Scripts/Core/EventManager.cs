using System;
using System.Collections.Generic;
using UnityEngine;

namespace WordPuzzle.Core
{
    public static class EventManager
    {
        private static Dictionary<string, Action<object>> _events = new Dictionary<string, Action<object>>();

        public static void StartListening(string eventName, Action<object> listener)
        {
            if (_events.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent += listener;
                _events[eventName] = thisEvent;
            }
            else
            {
                thisEvent += listener;
                _events.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, Action<object> listener)
        {
            if (_events.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent -= listener;
                _events[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName, object payload = null)
        {
            if (_events.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent?.Invoke(payload);
            }
        }
    }
}
