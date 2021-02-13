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
		public bool IsFilled { get { return CurrentFillCount >= m_maxFill; } }
		public int CurrentFillCount { get; private set; } = 0;
		public int ColumnIndex { get; private set; } = -1;

		private int m_maxFill = -1;

		private VerticalLayoutGroup m_layout = null;
		private EventTrigger m_eventTrigger = null;

		public void Config( int columnIndex, int maxFill )
		{
			ColumnIndex = columnIndex;
			m_maxFill = maxFill;
		}

		private void Start()
		{
			m_eventTrigger.AddEvent( EventTriggerType.PointerClick, OnClicked );
			m_eventTrigger.AddEvent( EventTriggerType.PointerEnter, OnPointerEntered );
		}

		private void OnClicked( BaseEventData data )
		{
			if ( IsFilled ) { return; }
			//Debug.Log( $"[{name}] | Clicked!" );

			GameGrid grid = GetComponentInParent<GameGrid>();
			Cell cell = grid.GetCell( CurrentFillCount, ColumnIndex );
			cell.Fill();

			++CurrentFillCount;
		}

		private void OnPointerEntered( BaseEventData data )
		{
			if ( IsFilled ) { return; }
			//Debug.Log( $"[{name}] | Pointer entered!" );
		}

		private void Awake()
		{
			m_layout = GetComponent<VerticalLayoutGroup>();
			m_eventTrigger = GetComponent<EventTrigger>();
		}
	}
}