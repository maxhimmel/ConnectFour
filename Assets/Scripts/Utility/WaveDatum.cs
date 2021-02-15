using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Utility
{
	[System.Serializable]
	public struct WaveDatum
	{
		public float Frequency;
		public float Amplitude;
		public float Phase;
		
		public WaveDatum( float frequency, float amplitude, float phase )
		{
			Frequency = frequency;
			Amplitude = amplitude;
			Phase = phase;
		}

		public float GetSinWave( float time )
		{
			return Amplitude * Mathf.Sin( Frequency * (time + Phase) );
		}

		public float GetCosWave( float time )
		{
			return Amplitude * Mathf.Cos( Frequency * (time + Phase) );
		}
	}
}