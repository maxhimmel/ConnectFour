using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour.Gameplay
{
	public class GameGrid : MonoBehaviour
	{
		private int TotalCellCount { get { return m_rowCount * m_columnCount; } }
		private RectTransform GridContainer { get { return transform as RectTransform; } }

		[Header( "Sizing" )]
		[SerializeField, Range( 3, 13 )] private int m_rowCount = 6;
		[SerializeField, Range( 3, 13 )] private int m_columnCount = 7;

		[Header( "References" )]
		[SerializeField] private Cell m_cellPrefab = default;
		[SerializeField] private Column m_columnPrefab = default;

		private Cell[] m_cells = null;
		private Column[] m_columns = null;

		public Cell GetCell( int rowIndex, int columnIndex )
		{
			if ( m_cells == null || m_cells.Length <= 0 ) { return null; }

			if ( rowIndex >= m_rowCount || columnIndex >= m_columnCount ) { return null; }
			if ( rowIndex < 0 || columnIndex < 0 ) { return null; }

			int idx = (columnIndex * m_rowCount) + rowIndex;
			return m_cells[idx];
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
					m_cells[idx] = CreateCell( col, row, newColumn.transform );
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
			newCell.name = $"Cell_[{rowIndex}, {columnIndex}]";
			
			newCell.SetSize( GetPreferredCellSize() );

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
	}
}