﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ConnectFour.Gameplay
{
	using Utility;
	using Animation;

	public class Column : MonoBehaviour
	{
		public bool IsFilled { get { return CurrentFillCount >= m_maxFill; } }
		public int CurrentFillCount { get; private set; } = 0;
		public int ColumnIndex { get; private set; } = -1;

		private GameManager Game { get { return GameManager.Instance; } }

		[Header( "Animation" )]
		[SerializeField] private float m_blinkSpeed = 1;
		[SerializeField] private Color m_normalColor = new Color( 1, 1, 1, 0 );
		[SerializeField] private Color m_highlightColor = new Color( 1, 1, 1, 0.2f );

		private int m_maxFill = -1;
		private EventTrigger m_eventTrigger = null;
		private ColorBlinker m_colorBlinker = null;

		public void Config( int columnIndex, int maxFill )
		{
			ColumnIndex = columnIndex;
			m_maxFill = maxFill;
		}

		private void Start()
		{
			m_eventTrigger.AddEvent( EventTriggerType.PointerClick, OnClicked );
			m_eventTrigger.AddEvent( EventTriggerType.PointerEnter, OnPointerEnter );
			m_eventTrigger.AddEvent( EventTriggerType.PointerExit, OnPointerExit );
		}

		private void OnClicked( BaseEventData data )
		{
			if ( IsFilled ) { return; }

			//GameGrid grid = GetComponentInParent<GameGrid>();
			//grid.PlacePiece( CurrentFillCount++, ColumnIndex );
			Game.PlacePiece( CurrentFillCount++, ColumnIndex );

			StartAnimatingColumn();
		}

		private void OnPointerEnter( BaseEventData data )
		{
			if ( IsFilled ) { return; }

			StartAnimatingColumn();
		}

		private void OnPointerExit( BaseEventData data )
		{
			if ( IsFilled ) { return; }

			StopAnimatingColumn();
		}

		private void StartAnimatingColumn()
		{
			m_colorBlinker.Play( m_blinkSpeed, m_normalColor, m_highlightColor );
			StartBlinkingNextPiece();
		}

		private void StartBlinkingNextPiece()
		{
			if ( IsFilled ) { return; }

			StopBlinkingNextPiece();
			
			Cell cell = Game.Grid.GetCell( CurrentFillCount, ColumnIndex );

			Color playerColor = Game.GetPlayerColor( Game.CurrentPlayer );
			Color startColor = playerColor;
			startColor.a = 0;

			cell.PlayTeaserFill( m_blinkSpeed, startColor, playerColor );
		}

		private void StopBlinkingNextPiece()
		{
			if ( IsFilled ) { return; }
			
			Cell cell = Game.Grid.GetCell( CurrentFillCount, ColumnIndex );
			cell.StopTeaserFill();
		}

		private void StopAnimatingColumn()
		{
			m_colorBlinker.Stop();
			m_colorBlinker.SetColor( m_normalColor );

			StopBlinkingNextPiece();
		}

		private void Awake()
		{
			m_eventTrigger = GetComponent<EventTrigger>();
			m_colorBlinker = GetComponent<ColorBlinker>();
		}
	}
}