using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Gameplay.Users
{
	public class RobotController : UserController
	{
		[SerializeField] private float m_teaseDuration = 0.7f;
		[SerializeField] private float m_pauseAfterTeaseDuration = 0.25f;

		[Space]
		[SerializeField, Min( 0 )] private int m_minTeases = 2;
		[SerializeField, Min( 0 )] private int m_maxTeases = 4;

		public override void StartTurn()
		{
			base.StartTurn();

			StartCoroutine( UpdateDecision() );
		}

		private IEnumerator UpdateDecision()
		{
			Column prevColumn = null;
			List<Column> columns = GetValidColumns();

			int teaseCount = Random.Range( m_minTeases, m_maxTeases + 1 );
			do
			{
				
				int randIdx = Random.Range( 0, columns.Count );
				Column randColumn = columns[randIdx];

				if ( teaseCount <= 0 )
				{
					// For the final choice let's always pick the best move ...
					randColumn = GetBestColumn( columns );
				}

				if ( prevColumn != randColumn )
				{
					if ( prevColumn != null )
					{
						prevColumn.Deselect();
					}

					SelectColumn( randColumn );
				}

				if ( m_teaseDuration > 0 || m_pauseAfterTeaseDuration > 0 )
				{
					yield return new WaitForSeconds( m_teaseDuration + m_pauseAfterTeaseDuration );
				}

				prevColumn = randColumn;

			} while ( --teaseCount > 0 );


			if ( prevColumn != null )
			{
				prevColumn.Deselect();
				prevColumn.PlacePiece();
			}
		}

		private List<Column> GetValidColumns()
		{
			List<Column> columns = new List<Column>();

			for ( int cdx = 0; cdx < Game.Rules.ColumnCount; ++cdx )
			{
				Column col = Game.Grid.GetColumn( cdx );
				if ( col.IsFilled ) { continue; }

				columns.Add( col );
			}

			return columns;
		}

		private Column GetBestColumn( List<Column> columns )
		{
			int randIdx = Random.Range( 0, columns.Count );
			return columns[randIdx];
		}

		private void SelectColumn( Column column )
		{
			column.Highlight();

			Vector2 columnPixelCoord = m_arm.GetCanvasPixelCoord( column.GetPixelCoord() );
			m_arm.StartMoving( m_teaseDuration, new Vector2()
			{
				x = columnPixelCoord.x,
				y = m_arm.AnchoredPos.y
			} );
		}
	}
}