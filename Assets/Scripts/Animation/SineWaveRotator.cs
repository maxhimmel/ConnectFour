using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Animation
{
	using Utility;

	public class SineWaveRotator : MonoBehaviour
	{
		public bool IsPlaying { get { return m_rotateRoutine != null; } }

		[SerializeField] private float m_maxAngle = 20;
		[SerializeField] private Vector3 m_axis = Vector3.forward;
		[SerializeField] private WaveDatum m_sineWave = new WaveDatum( 1, 1, 0 );

		private Coroutine m_rotateRoutine = null;

		[ContextMenu( "Play!" )]
		public void Play()
		{
			Stop();
			m_rotateRoutine = StartCoroutine( UpdateWave() );
		}

		[ContextMenu( "Stop, please" )]
		public void Stop()
		{
			if ( IsPlaying )
			{
				StopCoroutine( m_rotateRoutine );
				m_rotateRoutine = null;
			}
		}

		private IEnumerator UpdateWave()
		{
			float timer = 0;
			while ( enabled )
			{
				timer += Time.deltaTime;

				float sine = m_sineWave.GetSinWave( timer );

				Vector3 newEuler = m_axis * m_maxAngle * sine;
				transform.eulerAngles = newEuler;

				yield return null;
			}

			m_rotateRoutine = null;
		}

		private void OnDisable()
		{
			Stop();
		}
	}
}