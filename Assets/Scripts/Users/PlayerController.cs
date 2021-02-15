using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour.Gameplay.Users
{
	public class PlayerController : UserController
	{
		[SerializeField] protected float m_armDamping = 0.2f;

		private float m_armVelocity = 0;
		private Coroutine m_selectingRoutine = null;

		public override void StartTurn()
		{
			base.StartTurn();

			Game.Grid.SetInteractionActive( true );

			StopSelecting();
			m_selectingRoutine = StartCoroutine( UpdateSelecting() );
		}

		private void StopSelecting()
		{
			if ( m_selectingRoutine != null )
			{
				StopCoroutine( m_selectingRoutine );
				m_selectingRoutine = null;
			}
		}

		private IEnumerator UpdateSelecting()
		{
			while ( IsThinking )
			{
				Vector2 mousePos = Input.mousePosition;
				mousePos = m_arm.GetCanvasPixelCoord( mousePos );

				Vector2 currentArmPos = m_arm.AnchoredPos;
				currentArmPos.x = Mathf.SmoothDamp( currentArmPos.x, mousePos.x, ref m_armVelocity, m_armDamping );

				m_arm.SetAnchoredPosition( currentArmPos );

				yield return null;
			}

			m_selectingRoutine = null;
		}

		public override void EndTurn()
		{
			base.EndTurn();

			Game.Grid.SetInteractionActive( false );
		}

		private void OnDisable()
		{
			StopSelecting();
		}
	}
}