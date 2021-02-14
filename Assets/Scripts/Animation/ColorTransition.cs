using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour.Animation
{
	[RequireComponent( typeof( Graphic ) )]
	public class ColorTransition : MonoBehaviour
	{
		public bool IsPlaying { get { return m_transitionRoutine != null; } }

		[SerializeField] private bool m_ignoreTimescale = false;
		[SerializeField] private AnimationCurve m_animCurve = AnimationCurve.EaseInOut( 0, 0, 1, 1 );

		private Graphic m_graphic = null;
		private Coroutine m_transitionRoutine = null;

		public void Play( float duration, Color endColor )
		{
			Play( duration, m_graphic.color, endColor );
		}

		public void Play( float duration, Color startColor, Color endColor )
		{
			if ( !gameObject.activeInHierarchy ) { return; }

			if ( duration <= 0 )
			{
				SetColor( endColor );
				return;
			}

			Stop();
			m_transitionRoutine = StartCoroutine( UpdateTransition( duration, startColor, endColor ) );
		}

		public void Stop()
		{
			if ( IsPlaying )
			{
				StopCoroutine( m_transitionRoutine );
				m_transitionRoutine = null;
			}
		}

		private IEnumerator UpdateTransition( float duration, Color start, Color end )
		{
			float timer = 0;
			while ( timer < 1 )
			{
				timer += GetDeltaTime() / duration;
				timer = Mathf.Clamp01( timer );

				float sample = m_animCurve.Evaluate( timer );

				Color newColor = Color.LerpUnclamped( start, end, sample );
				SetColor( newColor );

				yield return null;
			}
			
			m_transitionRoutine = null;
		}

		public void SetColor( Color newColor )
		{
			m_graphic.color = newColor;
		}

		private float GetDeltaTime()
		{
			return m_ignoreTimescale
				? Time.unscaledDeltaTime
				: Time.deltaTime;
		}

		private void OnDisable()
		{
			Stop();
		}

		private void Awake()
		{
			m_graphic = GetComponent<Graphic>();
		}
	}
}