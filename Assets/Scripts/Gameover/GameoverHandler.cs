using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Gameplay
{
	using Utility;

	public class GameoverHandler : MonoBehaviour, IGameoverHandler
	{
		public event Action OnWonEvent;
		public event Action OnDrawEvent;

		private GameRules Rules { get { return Game.Rules; } }
		private int CurrentPlayer { get { return Game.CurrentPlayer; } }
		private GameManager Game { get { return GameManager.Instance; } }

		private int m_filledColumnCount = 0;

		bool IGameoverHandler.IsGameover( GameManager game )
		{
			bool isGameover = false;
			Vector2Int move = game.PrevPlacedPiece;

			TrackFilledColumns( move.Col() );

			if ( IsDrawGame() )
			{
				isGameover = true;

				OnDrawEvent?.Invoke();
			}
			else if ( IsWinner( move.Row(), move.Col() ) )
			{
				isGameover = true;

				OnWonEvent?.Invoke();
			}

			return isGameover;
		}

		private void TrackFilledColumns( int columnIndex )
		{
			Column column = Game.Grid.GetColumn( columnIndex );
			if ( column.IsFilled )
			{
				++m_filledColumnCount;
			}
		}

		private bool IsDrawGame()
		{
			return m_filledColumnCount >= Rules.ColumnCount;
		}

		private bool IsWinner( int rowIndex, int columnIndex )
		{
			if ( IsHorizontalWin( rowIndex, columnIndex ) ) { return true; }
			if ( IsVerticalWin( rowIndex, columnIndex ) ) { return true; }
			if ( IsAscendingDiagonalWin( rowIndex, columnIndex ) ) { return true; }
			if ( IsDescendingDiagonalWin( rowIndex, columnIndex ) ) { return true; }

			return false;
		}

		private bool IsHorizontalWin( int rowIndex, int columnIndex )
		{
			int connectedCellsCounter = 0;
			int winningConnectOffset = Rules.WinningConnectCount - 1;

			int minColumn = Mathf.Max( 0, columnIndex - winningConnectOffset );
			int maxColumn = Mathf.Min( Rules.ColumnCount - 1, columnIndex + winningConnectOffset );

			for ( int col = minColumn; col <= maxColumn; ++col )
			{
				Cell cell = Game.Grid.GetCell( rowIndex, col );
				if ( cell.PlayerOwner != CurrentPlayer )
				{
					connectedCellsCounter = 0;
					continue;
				}

				++connectedCellsCounter;
				if ( connectedCellsCounter >= Rules.WinningConnectCount ) { return true; }
			}

			return false;
		}

		private bool IsVerticalWin( int rowIndex, int columnIndex )
		{
			int connectedCellsCounter = 0;
			int winningConnectOffset = Rules.WinningConnectCount - 1;

			int minRow = Mathf.Max( 0, rowIndex - winningConnectOffset );
			int maxRow = Mathf.Min( Rules.RowCount - 1, rowIndex + winningConnectOffset );

			for ( int row = minRow; row <= maxRow; ++row )
			{
				Cell cell = Game.Grid.GetCell( row, columnIndex );
				if ( cell.PlayerOwner != CurrentPlayer )
				{
					connectedCellsCounter = 0;
					continue;
				}

				++connectedCellsCounter;
				if ( connectedCellsCounter >= Rules.WinningConnectCount ) { return true; }
			}

			return false;
		}

		private bool IsAscendingDiagonalWin( int rowIndex, int columnIndex )
		{
			int connectedCellsCounter = 0;
			int winningConnectOffset = Rules.WinningConnectCount - 1;

			int minColumn = Mathf.Max( 0, columnIndex - winningConnectOffset, columnIndex - rowIndex );
			int maxColumn = Mathf.Min( Rules.ColumnCount - 1, columnIndex + winningConnectOffset, columnIndex + ((Rules.RowCount - 1) - rowIndex) );
			int minRow = Mathf.Max( 0, rowIndex - winningConnectOffset, rowIndex - columnIndex );
			int maxRow = Mathf.Min( Rules.RowCount - 1, rowIndex + winningConnectOffset, rowIndex + ((Rules.ColumnCount - 1) - columnIndex) );

			for ( int row = minRow, col = minColumn; row <= maxRow && col <= maxColumn; ++row, ++col )
			{
				Cell cell = Game.Grid.GetCell( row, col );
				if ( cell.PlayerOwner != CurrentPlayer )
				{
					connectedCellsCounter = 0;
					continue;
				}

				++connectedCellsCounter;
				if ( connectedCellsCounter >= Rules.WinningConnectCount ) { return true; }
			}

			return false;
		}

		private bool IsDescendingDiagonalWin( int rowIndex, int columnIndex )
		{
			int connectedCellsCounter = 0;
			int winningConnectOffset = Rules.WinningConnectCount - 1;

			int minColumn = Mathf.Max( 0, columnIndex - winningConnectOffset, columnIndex - ((Rules.RowCount - 1) - rowIndex) );
			int maxColumn = Mathf.Min( Rules.ColumnCount - 1, columnIndex + winningConnectOffset, columnIndex + rowIndex );
			int minRow = Mathf.Max( 0, rowIndex - winningConnectOffset, rowIndex - ((Rules.ColumnCount - 1) - columnIndex) );
			int maxRow = Mathf.Min( Rules.RowCount - 1, rowIndex + winningConnectOffset, rowIndex + columnIndex );

			for ( int row = maxRow, col = minColumn; row >= minRow && col <= maxColumn; --row, ++col )
			{
				Cell cell = Game.Grid.GetCell( row, col );
				if ( cell.PlayerOwner != CurrentPlayer )
				{
					connectedCellsCounter = 0;
					continue;
				}

				++connectedCellsCounter;
				if ( connectedCellsCounter >= Rules.WinningConnectCount ) { return true; }
			}

			return false;
		}
	}
}