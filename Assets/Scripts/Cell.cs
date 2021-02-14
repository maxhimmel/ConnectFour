using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour.Gameplay
{
	using Utility;

	public class Cell : MonoBehaviour
	{
		public const int k_nonePlayer = -1;

		public Vector2Int Index { get; private set; }
		public int PlayerOwner { get; private set; } = k_nonePlayer;

		[SerializeField] private Image m_fill = default;

		private LayoutElement m_layout = null;
		private Color m_emptyColor = Color.white;
		private List<Vector2Int> m_adjacentCells = new List<Vector2Int>();

		public void Config( float newSize, int row, int column, List<Vector2Int> adjacentCells )
		{
			m_layout.preferredHeight = m_layout.preferredWidth = newSize;
			Index = GridHelper.RowColVector( row, column );
			m_adjacentCells = adjacentCells;
		}

		private void OnEnable()
		{
			Empty();
		}

		public void Empty()
		{
			m_fill.enabled = false;
			m_fill.color = m_emptyColor;
			PlayerOwner = k_nonePlayer;
		}

		public void Fill( int player )
		{
			m_fill.enabled = true;
			PlayerOwner = player;

			GameGrid grid = GetComponentInParent<GameGrid>();
			m_fill.color = grid.GetPlayerColor( player );
		}

		private void Awake()
		{
			m_layout = GetComponent<LayoutElement>();
			m_emptyColor = m_fill.color;
		}


#if UNITY_EDITOR
		[Header( "Editor / Tools" )]
		[SerializeField] private Color m_adjacentCellColor = Color.magenta;

		private void OnDrawGizmosSelected()
		{
			DrawAdjacentCells();
		}

		private void DrawAdjacentCells()
		{
			if ( m_adjacentCells != null )
			{
				GameGrid grid = GetComponentInParent<GameGrid>();

				foreach ( Vector2Int adjacentIdx in m_adjacentCells )
				{
					Cell adjacentCell = grid.GetCell( adjacentIdx.Row(), adjacentIdx.Col() );

					RectTransform adjacentCellRect = adjacentCell.transform as RectTransform;
					Vector3 pos = adjacentCellRect.position;
					Vector3 size = adjacentCellRect.rect.size * adjacentCellRect.lossyScale * 0.85f;

					Gizmos.color = m_adjacentCellColor;
					Gizmos.DrawWireCube( pos, size );
				}
			}
		}
#endif
	}
}