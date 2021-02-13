using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ConnectFour.Gameplay
{
	using Utility;

	public class Column : MonoBehaviour
	{
		public int CurrentFillCount { get; private set; }

		private int m_maxFill = -1;

		private VerticalLayoutGroup m_layout = null;
		private EventTrigger m_eventTrigger = null;

		public void SetMaxFillCount( int maxFill )
		{
			m_maxFill = maxFill;
		}

		private void Start()
		{
			m_eventTrigger.AddEvent( EventTriggerType.PointerClick, OnClicked );
			m_eventTrigger.AddEvent( EventTriggerType.PointerEnter, OnPointerEntered );
		}

		private void OnClicked( BaseEventData data )
		{
			//Debug.Log( $"[{name}] | Clicked!" );
		}

		private void OnPointerEntered( BaseEventData data )
		{
			//Debug.Log( $"[{name}] | Pointer entered!" );
		}

		private void Awake()
		{
			m_layout = GetComponent<VerticalLayoutGroup>();
			m_eventTrigger = GetComponent<EventTrigger>();
		}
	}
}