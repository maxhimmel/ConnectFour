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

		[Space]
		[SerializeField] private Transform m_rotationPivot = default;

		private Coroutine m_rotateRoutine = null;
		private Vector3 m_initialPivot;

		[ContextMenu( "Play!" )]
		public void Play()
		{
			Stop();

			ResetInitialPivotPoint();
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

		public void ResetInitialPivotPoint()
		{
			m_initialPivot = transform.position;
		}

		private IEnumerator UpdateWave()
		{
			float timer = 0;
			while ( enabled )
			{
				timer += Time.deltaTime;
				float sine = m_sineWave.GetSinWave( timer );
				Vector3 newEuler = m_axis * m_maxAngle * sine;

				transform.position = RotatePointAroundPivot( m_initialPivot, GetPivot(), newEuler  );
				transform.rotation = Quaternion.Euler( newEuler );

				yield return null;
			}

			m_rotateRoutine = null;
		}

		private Vector3 RotatePointAroundPivot( Vector3 point, Vector3 pivot, Vector3 eulerAngles )
		{
			return Quaternion.Euler( eulerAngles ) * (point - pivot) + pivot;
		}

		private Vector3 GetPivot()
		{
			return m_rotationPivot != null
				? m_rotationPivot.position
				: transform.position;
		}
		
		public void SetAmplitude( float amplitude )
		{
			m_sineWave.Amplitude = amplitude;
		}

		private void OnDisable()
		{
			Stop();
		}

		private void Start()
		{
			ResetInitialPivotPoint();
		}
	}
}