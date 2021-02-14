using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour.Gameplay
{
	public class GameGrid : MonoBehaviour
	{
		private RectTransform GridContainer { get { return m_container.transform as RectTransform; } }
		private GameRules Rules { get { return Game.Rules; } }
		private GameManager Game { get { return GameManager.Instance; } }

		[SerializeField] private HorizontalLayoutGroup m_container = default;

		[Header( "References" )]
		[SerializeField] private Cell m_cellPrefab = default;
		[SerializeField] private Column m_columnPrefab = default;

		private Cell[] m_cells = null;
		private Column[] m_columns = null;
		private CanvasGroup m_canvasGroup = null;

		public void SetInteractionActive( bool isActive )
		{
			m_canvasGroup.interactable = isActive;
			m_canvasGroup.blocksRaycasts = isActive;
		}

		public Cell GetCell( int rowIndex, int columnIndex )
		{
			if ( m_cells == null || m_cells.Length <= 0 ) { return null; }

			if ( rowIndex >= Rules.RowCount || columnIndex >= Rules.ColumnCount ) { return null; }
			if ( rowIndex < 0 || columnIndex < 0 ) { return null; }

			int idx = (columnIndex * Rules.RowCount) + rowIndex;
			return m_cells[idx];
		}

		public Column GetColumn( int columnIndex )
		{
			if ( m_columns == null || m_columns.Length <= 0 ) { return null; }
			if ( columnIndex < 0 || columnIndex >= m_columns.Length ) { return null; }

			return m_columns[columnIndex];
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
			m_cells = new Cell[Rules.TotalCellCount];
			m_columns = new Column[Rules.ColumnCount];

			for ( int col = 0; col < Rules.ColumnCount; ++col )
			{
				Column newColumn = CreateColumn( col, Rules.RowCount );
				m_columns[col] = newColumn;

				for ( int row = 0; row < Rules.RowCount; ++row )
				{
					int idx = (col * Rules.RowCount) + row;
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

			newCell.Config( GetPreferredCellSize(), rowIndex, columnIndex );

			return newCell;
		}

		private float GetPreferredCellSize()
		{
			RectTransform gridRect = transform as RectTransform;
			Vector2 gridSize = gridRect.rect.size;
			Vector2 maxCellSize = new Vector2()
			{
				x = gridSize.x / Rules.ColumnCount,
				y = gridSize.y / Rules.RowCount
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
			if ( m_columns != null )
			{
				for ( int cdx = 0; cdx < m_columns.Length; ++cdx )
				{
					Column column = m_columns[cdx];
					Destroy( column.gameObject );
				}
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