using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour.Gameplay
{
	using Utility;

	public class GameGrid : MonoBehaviour
	{
		public const int k_playerCount = 2;

		public int CurrentPlayer { get; private set; }

		private int TotalCellCount { get { return m_rowCount * m_columnCount; } }
		private RectTransform GridContainer { get { return transform as RectTransform; } }

		[Header( "Rules" )]
		[SerializeField, Range( 2, 9 )] private int m_winningConnectCount = 4;

		[Space]
		[SerializeField] private Color[] m_playerColors = new Color[k_playerCount] { Color.red, Color.blue };

		[Space]
		[SerializeField, Range( 3, 13 )] private int m_rowCount = 6;
		[SerializeField, Range( 3, 13 )] private int m_columnCount = 7;

		[Header( "References" )]
		[SerializeField] private Cell m_cellPrefab = default;
		[SerializeField] private Column m_columnPrefab = default;

		private Cell[] m_cells = null;
		private Column[] m_columns = null;
		private int m_filledColumnCount = 0;
		private CanvasGroup m_canvasGroup = null;

		public void SetInteractionActive( bool isActive )
		{
			m_canvasGroup.interactable = isActive;
			m_canvasGroup.blocksRaycasts = isActive;
		}

		public void PlacePiece( int rowIndex, int columnIndex )
		{
			Cell cell = GetCell( rowIndex, columnIndex );
			if ( cell != null )
			{
				cell.Fill( CurrentPlayer );
			}

			Column column = m_columns[columnIndex];
			if ( column != null && column.IsFilled )
			{
				++m_filledColumnCount;
			}

			if ( !HandleGameover( rowIndex, columnIndex ) )
			{
				CycleToNextTurn();
			}
			//else
			//{
			//	SetInteractionActive( false );
			//}
		}

		private bool HandleGameover( int rowIndex, int columnIndex )
		{
			if ( IsWinner( rowIndex, columnIndex ) )
			{
				Debug.Log( $"Player {CurrentPlayer} has won!", this );

				// TODO: Remove me (used to cycle players even when winning to debug/play easier
				CycleToNextTurn();

				return true;
			}
			else if ( IsDrawGame() )
			{
				Debug.Log( $"Draw game!", this );
				return true;
			}

			return false;
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
			int winningConnectOffset = m_winningConnectCount - 1;

			int minColumn = Mathf.Max( 0, columnIndex - winningConnectOffset );
			int maxColumn = Mathf.Min( m_columnCount - 1, columnIndex + winningConnectOffset );

			for ( int col = minColumn; col <= maxColumn; ++col )
			{
				Cell cell = GetCell( rowIndex, col );
				if ( cell.PlayerOwner != CurrentPlayer )
				{
					connectedCellsCounter = 0;
					continue;
				}

				++connectedCellsCounter;
				if ( connectedCellsCounter >= m_winningConnectCount ) { return true; }
			}

			return false;
		}

		private bool IsVerticalWin( int rowIndex, int columnIndex )
		{
			int connectedCellsCounter = 0;
			int winningConnectOffset = m_winningConnectCount - 1;

			int minRow = Mathf.Max( 0, rowIndex - winningConnectOffset );
			int maxRow = Mathf.Min( m_rowCount - 1, rowIndex + winningConnectOffset );

			for ( int row = minRow; row <= maxRow; ++row )
			{
				Cell cell = GetCell( row, columnIndex );
				if ( cell.PlayerOwner != CurrentPlayer )
				{
					connectedCellsCounter = 0;
					continue;
				}

				++connectedCellsCounter;
				if ( connectedCellsCounter >= m_winningConnectCount ) { return true; }
			}

			return false;
		}

		private bool IsAscendingDiagonalWin( int rowIndex, int columnIndex )
		{
			int connectedCellsCounter = 0;
			int winningConnectOffset = m_winningConnectCount - 1;

			int minColumn = Mathf.Max( 0, columnIndex - winningConnectOffset, columnIndex - rowIndex );
			int maxColumn = Mathf.Min( m_columnCount - 1, columnIndex + winningConnectOffset, columnIndex + ((m_rowCount - 1) - rowIndex) );			
			int minRow = Mathf.Max( 0, rowIndex - winningConnectOffset, rowIndex - columnIndex );
			int maxRow = Mathf.Min( m_rowCount - 1, rowIndex + winningConnectOffset, rowIndex + ((m_columnCount - 1) - columnIndex) );

			for ( int row = minRow, col = minColumn; row <= maxRow && col <= maxColumn; ++row, ++col )
			{
				Cell cell = GetCell( row, col );
				if ( cell.PlayerOwner != CurrentPlayer )
				{
					connectedCellsCounter = 0;
					continue;
				}

				++connectedCellsCounter;
				if ( connectedCellsCounter >= m_winningConnectCount ) { return true; }
			}

			return false;
		}

		private bool IsDescendingDiagonalWin( int rowIndex, int columnIndex )
		{
			int connectedCellsCounter = 0;
			int winningConnectOffset = m_winningConnectCount - 1;

			int minColumn = Mathf.Max( 0, columnIndex - winningConnectOffset, columnIndex - ((m_rowCount - 1) - rowIndex) );
			int maxColumn = Mathf.Min( m_columnCount - 1, columnIndex + winningConnectOffset, columnIndex + rowIndex );
			int minRow = Mathf.Max( 0, rowIndex - winningConnectOffset, rowIndex - ((m_columnCount - 1) - columnIndex) );
			int maxRow = Mathf.Min( m_rowCount - 1, rowIndex + winningConnectOffset, rowIndex + columnIndex );
			
			for ( int row = maxRow, col = minColumn; row >= minRow && col <= maxColumn; --row, ++col )
			{
				Cell cell = GetCell( row, col );
				if ( cell.PlayerOwner != CurrentPlayer )
				{
					connectedCellsCounter = 0;
					continue;
				}

				++connectedCellsCounter;
				if ( connectedCellsCounter >= m_winningConnectCount ) { return true; }
			}

			return false;
		}

		private bool IsDrawGame()
		{
			return m_filledColumnCount >= m_columnCount;
		}

		private void CycleToNextTurn()
		{
			CurrentPlayer = GetNextPlayer( CurrentPlayer );
		}

		private int GetNextPlayer( int currentPlayer )
		{
			return ++currentPlayer % k_playerCount;
		}

		public Color GetPlayerColor( int player )
		{
			if ( m_playerColors == null || m_playerColors.Length <= 0 ) { return Color.magenta; }
			if ( player < 0 || player >= m_playerColors.Length ) { return Color.magenta; }

			return m_playerColors[player];
		}

		public Cell GetCell( int rowIndex, int columnIndex )
		{
			if ( m_cells == null || m_cells.Length <= 0 ) { return null; }

			if ( rowIndex >= m_rowCount || columnIndex >= m_columnCount ) { return null; }
			if ( rowIndex < 0 || columnIndex < 0 ) { return null; }

			int idx = (columnIndex * m_rowCount) + rowIndex;
			return m_cells[idx];
		}

		public List<Vector2Int> GetAdjacentCellIndices( int rowIndex, int columnIndex )
		{
			List<Vector2Int> results = new List<Vector2Int>();
			
			for ( int col = columnIndex - 1; col <= columnIndex + 1; ++col )
			{
				for ( int row = rowIndex - 1; row <= rowIndex + 1; ++row )
				{
					if ( col < 0 || col >= m_columnCount ) { continue; }
					if ( row < 0 || row >= m_rowCount ) { continue; }
					if ( row == rowIndex && col == columnIndex ) { continue; }

					results.Add( GridHelper.RowColVector( row, col ) );
				}
			}
			
			return results;
		}

		private void OnEnable()
		{
			if ( Application.isPlaying )
			{
				CreateGrid();
			}
		}

		private void CreateGrid()
		{
			m_cells = new Cell[TotalCellCount];
			m_columns = new Column[m_columnCount];

			for ( int col = 0; col < m_columnCount; ++col )
			{
				Column newColumn = CreateColumn( col, m_rowCount );
				m_columns[col] = newColumn;

				for ( int row = 0; row < m_rowCount; ++row )
				{
					int idx = (col * m_rowCount) + row;
					m_cells[idx] = CreateCell( row, col, newColumn.transform );
				}
			}
		}

		private Column CreateColumn( int columnIndex, int totalRows )
		{
			Column newColumn = Instantiate( m_columnPrefab, GridContainer );
			newColumn.name = $"Column_{columnIndex}";

			newColumn.Config( columnIndex, totalRows );

			return newColumn;
		}

		private Cell CreateCell( int rowIndex, int columnIndex, Transform group )
		{
			Cell newCell = Instantiate( m_cellPrefab, group );
			newCell.transform.SetAsFirstSibling();
			newCell.name = $"Cell_[r.{rowIndex}, c.{columnIndex}]";

			newCell.Config( GetPreferredCellSize(), rowIndex, columnIndex, GetAdjacentCellIndices( rowIndex, columnIndex ) );

			return newCell;
		}

		private float GetPreferredCellSize()
		{
			Vector2 gridSize = GridContainer.rect.size;
			Vector2 maxCellSize = new Vector2()
			{
				x = gridSize.x / m_columnCount,
				y = gridSize.y / m_rowCount
			};

			return Mathf.Min( maxCellSize.x, maxCellSize.y );
		}

		private void OnDisable()
		{
			if ( Application.isPlaying )
			{
				DestroyGrid();
			}
		}

		private void DestroyGrid()
		{
			int childCount = GridContainer.childCount;
			for ( int idx = 0; idx < childCount; ++idx )
			{
				Destroy( GridContainer.GetChild( idx ).gameObject );
			}

			m_cells = null;
			m_columns = null;
		}

		private void Awake()
		{
			m_canvasGroup = GetComponent<CanvasGroup>();
		}
	}
}