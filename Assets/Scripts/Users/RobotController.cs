using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Gameplay.Users
{
	public class RobotController : UserController
	{
		[SerializeField] private float m_decisionDelay = 0.5f;
		[SerializeField] private float m_teaseDuration = 0.5f;

		public override void StartTurn()
		{
			base.StartTurn();

			StartCoroutine( UpdateDecision() );
		}

		private IEnumerator UpdateDecision()
		{
			List<Column> columns = GetValidColumns();

			float decisionTimer = 0;
			float teaseTimer = 0;

			Column randColumn = columns[Random.Range( 0, columns.Count )];
			randColumn.Highlight();

			while ( decisionTimer < 1 )
			{
				decisionTimer += Time.deltaTime / m_decisionDelay;
				decisionTimer = Mathf.Clamp01( decisionTimer );

				teaseTimer += Time.deltaTime / m_teaseDuration;
				teaseTimer = Mathf.Clamp01( teaseTimer );

				if ( teaseTimer >= 1 )
				{
					teaseTimer = 0;

					Column prevColumn = randColumn;
					randColumn = columns[Random.Range( 0, columns.Count )];

					if ( randColumn != prevColumn )
					{
						prevColumn.Deselect();
						randColumn.Highlight();
					}
				}

				yield return null;
			}

			randColumn.Deselect();
			randColumn.PlacePiece();
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
	}
}