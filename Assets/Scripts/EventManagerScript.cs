using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManagerScript : Singleton<EventManagerScript>
{
    protected EventManagerScript()
    {
        Init();
    } // guarantee this will be always a singleton only - can't use the constructor!

    public class FloatEvent : UnityEvent<object> {} //empty class; just needs to exist

    public const string EVENT__CATCH_DIAMOND = "event_catchDiamond";

    private Dictionary <string, FloatEvent> eventDictionary;
	
	private void Init ()
	{
		if (eventDictionary == null)
		{
			eventDictionary = new Dictionary<string, FloatEvent>();
		}
	}
	
	public void StartListening (string eventName, UnityAction<object> listener)
	{
		FloatEvent thisEvent = null;
		if (eventDictionary.TryGetValue (eventName, out thisEvent))
		{
			thisEvent.AddListener (listener);
		} 
		else
		{
			thisEvent = new FloatEvent ();
			thisEvent.AddListener (listener);
			eventDictionary.Add (eventName, thisEvent);
		}
	}
	
	public void StopListening (string eventName, UnityAction<object> listener)
	{
		FloatEvent thisEvent = null;
		if (eventDictionary.TryGetValue (eventName, out thisEvent))
		{
			thisEvent.RemoveListener (listener);
		}
	}
	
	public void TriggerEvent (string eventName, object obj)
	{
		FloatEvent thisEvent = null;
		if (eventDictionary.TryGetValue (eventName, out thisEvent))
		{
			thisEvent.Invoke (obj);
		}
	}
}
