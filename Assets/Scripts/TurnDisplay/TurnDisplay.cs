using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour.Gameplay
{
	public class TurnDisplay : MonoBehaviour
	{
		private GameManager Game { get { return GameManager.Instance; } }
		
		[SerializeField] private Image m_display = default;

		[Space]
		[SerializeField] private float m_rotateDuration = 0.7f;
		[SerializeField] private AnimationCurve m_rotateCurve = AnimationCurve.EaseInOut( 0, 0, 1, 1 );

		private Coroutine m_showHideRoutine = null;

		public void SetColor( int player )
		{
			m_display.color = Game.GetPlayerColor( player );
		}

		public void Show()
		{
			Stop();
			m_showHideRoutine = StartCoroutine( RotateForSeconds( m_rotateDuration, 0 ) );
		}

		public void Hide()
		{
			Stop();
			m_showHideRoutine = StartCoroutine( RotateForSeconds( m_rotateDuration, 180 ) );
		}

		private void Stop()
		{
			if ( m_showHideRoutine != null )
			{
				StopCoroutine( m_showHideRoutine );
				m_showHideRoutine = null;
			}
		}

		private IEnumerator RotateForSeconds( float duration, float targetAngle )
		{
			float startAngle = transform.eulerAngles.z;

			float timer = 0;
			while ( timer < 1 )
			{
				timer += Time.deltaTime / duration;
				timer = Mathf.Clamp01( timer );

				float sample = m_rotateCurve.Evaluate( timer );
				float newAngle = Mathf.LerpUnclamped( startAngle, targetAngle, sample );

				transform.eulerAngles = new Vector3()
				{
					x = transform.eulerAngles.x,
					y = transform.eulerAngles.y,
					z = newAngle
				};

				yield return null;
			}

			m_showHideRoutine = null;
		}

		private void OnDisable()
		{
			Stop();
		}
	}
}