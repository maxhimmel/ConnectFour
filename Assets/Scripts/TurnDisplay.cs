using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour.Gameplay
{
	public class TurnDisplay : MonoBehaviour
	{
		private GameManager Game { get { return GameManager.Instance; } }

		[SerializeField] private RectTransform[] m_turnDisplay = new RectTransform[GameRules.k_playerCount];

		[Space]
		[SerializeField] private float m_rotateDuration = 0.7f;
		[SerializeField] private AnimationCurve m_rotateCurve = AnimationCurve.EaseInOut( 0, 0, 1, 1 );

		private void Start()
		{
			Game.OnPlayerTurnChangedEvent += OnPlayerTurnChanged;

			SetTurnDisplayColor( 0 );
			SetTurnDisplayColor( 1 );
		}

		private void SetTurnDisplayColor( int index )
		{
			Transform banner = m_turnDisplay[index].GetChild( 1 );
			Image bannerImage = banner.GetComponent<Image>();
			bannerImage.color = Game.GetPlayerColor( index );
		}

		private void OnPlayerTurnChanged( int player )
		{
			RectTransform display = m_turnDisplay[player];
			StartCoroutine( RotateForSeconds( display, m_rotateDuration, 0 ) );

			RectTransform other = m_turnDisplay[++player % GameRules.k_playerCount];
			StartCoroutine( RotateForSeconds( other, m_rotateDuration, 180f ) );
		}

		private IEnumerator RotateForSeconds( RectTransform display, float duration, float targetAngle )
		{
			float startAngle = display.eulerAngles.z;

			float timer = 0;
			while ( timer < 1 )
			{
				timer += Time.deltaTime / duration;
				timer = Mathf.Clamp01( timer );

				float sample = m_rotateCurve.Evaluate( timer );
				float newAngle = Mathf.LerpUnclamped( startAngle, targetAngle, sample );

				display.eulerAngles = new Vector3()
				{
					x = display.eulerAngles.x,
					y = display.eulerAngles.y,
					z = newAngle
				};

				yield return null;
			}
		}
	}
}