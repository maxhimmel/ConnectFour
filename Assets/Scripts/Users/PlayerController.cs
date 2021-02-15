using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour.Gameplay.Users
{
	public class PlayerController : UserController
	{
		[Header( "Player Control" )]
		[SerializeField] private float m_armDamping = 0.2f;
		[SerializeField] private float m_celebrationDamping = 0.1f;
		[SerializeField] private float m_maxCelebrationStrength = 5f;

		private float m_armVelocity = 0;
		private Vector3 m_celebrationVelocity = Vector3.zero;
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

		protected override void OnWon()
		{
			base.OnWon();

			IsThinking = false;
			StopSelecting();

			StartCoroutine( UpdateCelebration() );
		}

		private IEnumerator UpdateCelebration()
		{
			Vector3 prevMousePos = Input.mousePosition;
			Vector3 smoothMousePos = prevMousePos;

			while ( enabled )
			{
				Vector3 currentMousePos = Input.mousePosition;
				smoothMousePos = Vector3.SmoothDamp( smoothMousePos, currentMousePos, ref m_celebrationVelocity, m_celebrationDamping );

				Vector2 mouseDelta = Vector3.Max( smoothMousePos, prevMousePos ) - Vector3.Min( smoothMousePos, prevMousePos );
				float mouseSpeed = Mathf.Min( mouseDelta.magnitude, m_maxCelebrationStrength );

				prevMousePos = smoothMousePos;
				m_arm.SetCelebrationStrength( mouseSpeed );

				yield return null;
			}
		}

		private void OnDisable()
		{
			StopSelecting();
		}
	}
}