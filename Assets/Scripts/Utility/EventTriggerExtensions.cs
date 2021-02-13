using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ConnectFour.Utility
{
	public static class EventTriggerExtensions
	{
		public static void AddEvent( this EventTrigger trigger, EventTriggerType type, UnityAction<BaseEventData> action )
		{
			if ( !trigger.TryGetEventEntry( type, out EventTrigger.Entry eventEntry ) )
			{
				trigger.triggers.Add( eventEntry );
			}

			eventEntry.callback.AddListener( action );
		}

		public static bool TryGetEventEntry( this EventTrigger trigger, EventTriggerType type, out EventTrigger.Entry entry )
		{
			entry = trigger.triggers.Find( e => e.eventID == type );
			if ( entry != null ) { return true; }

			entry = new EventTrigger.Entry();
			entry.eventID = type;

			return false;
		}
	}
}