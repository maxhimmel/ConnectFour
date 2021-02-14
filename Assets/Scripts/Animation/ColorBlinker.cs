using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Animation
{
	[RequireComponent( typeof( ColorTransition ) )]
	public class ColorBlinker : MonoBehaviour
	{
		public bool IsPlaying { get { return m_blinkRoutine != null; } }

		private ColorTransition m_transition = null;
		private Coroutine m_blinkRoutine = null;

		public void SetColor( Color newColor )
		{
			m_transition.SetColor( newColor );
		}

		public void Play( float duration, Color firstColor, Color secondColor )
		{
			Stop();
			m_blinkRoutine = StartCoroutine( UpdateBlinking( duration, firstColor, secondColor ) );
		}

		public void Stop()
		{
			if ( m_blinkRoutine != null )
			{
				StopCoroutine( m_blinkRoutine );
				m_blinkRoutine = null;
			}

			m_transition.Stop();
		}

		private IEnumerator UpdateBlinking( float duration, Color first, Color second )
		{
			do
			{

				m_transition.Play( duration, first, second );
				while ( m_transition.IsPlaying ) { yield return null; }

				Color tempFirst = first;
				first = second;
				second = tempFirst;

			} while ( enabled );

			m_blinkRoutine = null;
		}

		private void OnDisable()
		{
			Stop();
		}

		private void Awake()
		{
			m_transition = GetComponent<ColorTransition>();
		}
	}
}