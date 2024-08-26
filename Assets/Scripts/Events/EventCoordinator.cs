using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCoordinator<EventType> where EventType : EventInfo
{
    delegate void EventListener(EventInfo ei);
    [SerializeField] private static Dictionary<Type, Action<EventType>> _typeToActionDictionary;

    //static private EventCoordinator _current;

    static EventCoordinator()
    {
        _typeToActionDictionary = new Dictionary<Type, Action<EventType>>();
    }


    public static void RegisterListener(Action<EventType> listener)
    {
        RegisterType();
        _typeToActionDictionary[typeof(EventType)] += listener;
    }
    public static void UnregisterListener(Action<EventType> listener)
    {
        RegisterType();
        _typeToActionDictionary[typeof(EventType)] -= listener;
    }


    private static void RegisterType()
    {
        //if the dictionary is empty create a keyset with the Type and Action.
        if (_typeToActionDictionary == null)
        {
            _typeToActionDictionary = new Dictionary<Type, Action<EventType>>();
        }
        //If there is no category for listeners of this type, or if the category does not have a value, then create a new list for the category.
        if (_typeToActionDictionary.ContainsKey(typeof(EventType)) == false)
        {
            _typeToActionDictionary.Add(typeof(EventType), null);
        }


    }

    //what happens when an event is called.
    public static void FireEvent(EventType eventType)
    {
        //if the given eventType does not exist, then return.
        if (_typeToActionDictionary == null || !_typeToActionDictionary.ContainsKey(typeof(EventType)))
        {
            return;
        }

        //if the given eventType does exist, then fire it.
        _typeToActionDictionary[typeof(EventType)]?.Invoke(eventType);

    }

}