using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour.Animation
{
	[RequireComponent( typeof( CanvasGroup ) )]
	public class CanvasGroupFader : MonoBehaviour
	{
		public bool IsPlaying { get { return m_fadeRoutine != null; } }

		[SerializeField] private bool m_ignoreTimescale = false;

		private CanvasGroup m_canvasGroup = null;
		private Coroutine m_fadeRoutine = null;

		public void FadeIn( float duration )
		{
			Stop();
			m_fadeRoutine = StartCoroutine( UpdateFading( duration, 1 ) );
		}

		public void FadeOut( float duration )
		{
			Stop();
			m_fadeRoutine = StartCoroutine( UpdateFading( duration, 0 ) );
		}

		private void Stop()
		{
			if ( IsPlaying )
			{
				StopCoroutine( m_fadeRoutine );
				m_fadeRoutine = null;
			}
		}

		private IEnumerator UpdateFading( float duration, float alpha )
		{
			float currentAlpha = m_canvasGroup.alpha;
			float start = (currentAlpha < alpha) ? 0 : 1;
			float end = (currentAlpha < alpha) ? 1 : 0;

			float timer = Mathf.InverseLerp( start, end, currentAlpha );
			while ( timer < 1 )
			{
				timer += GetDeltaTime() / duration;
				timer = Mathf.Clamp01( timer );

				float nextAlpha = Mathf.LerpUnclamped( start, end, timer );
				m_canvasGroup.alpha = nextAlpha;

				yield return null;
			}

			m_fadeRoutine = null;
		}

		private float GetDeltaTime()
		{
			return m_ignoreTimescale
				? Time.unscaledDeltaTime
				: Time.deltaTime;
		}

		private void Awake()
		{
			m_canvasGroup = GetComponent<CanvasGroup>();
		}
	}
}