using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Gameplay
{
	public class ArmAnimator : MonoBehaviour
	{
		public Vector2 AnchoredPos { get { return m_rectTransform.anchoredPosition; } }
		public bool IsMoving { get { return m_movementRoutine != null; } }

		[SerializeField] private AnimationCurve m_moveAnimCurve = AnimationCurve.EaseInOut( 0, 0, 1, 1 );

		private RectTransform m_rectTransform = null;
		private RectTransform m_canvasRect = null;
		private Coroutine m_movementRoutine = null;

		public Vector2 GetCanvasPixelCoord( Vector2 pixelCoord )
		{
			return new Vector2()
			{
				x = pixelCoord.x / Screen.width * m_canvasRect.rect.width,
				y = pixelCoord.y / Screen.height * m_canvasRect.rect.height
			};
		}

		public void SetAnchoredPosition( Vector2 pixelCoord )
		{
			m_rectTransform.anchoredPosition = pixelCoord;
		}

		public void StartMoving( float duration, Vector2 pixelCoord )
		{
			StopMoving();
			m_movementRoutine = StartCoroutine( UpdateMovement( duration, pixelCoord ) );
		}

		private void StopMoving()
		{
			if ( IsMoving )
			{
				StopCoroutine( m_movementRoutine );
				m_movementRoutine = null;
			}
		}

		private IEnumerator UpdateMovement( float duration, Vector2 pixelCoord )
		{
			Vector2 startPos = m_rectTransform.anchoredPosition;

			float timer = 0;
			while ( timer < 1 )
			{
				timer += Time.deltaTime / duration;
				timer = Mathf.Clamp01( timer );

				float sample = m_moveAnimCurve.Evaluate( timer );
				Vector2 newPos = Vector2.LerpUnclamped( startPos, pixelCoord, sample );

				SetAnchoredPosition( newPos );
				yield return null;
			}

			m_movementRoutine = null;
		}

		private void OnDisable()
		{
			StopMoving();
		}

		private void Awake()
		{
			m_rectTransform = GetComponent<RectTransform>();

			Canvas parent = GetComponentInParent<Canvas>();
			m_canvasRect = parent.transform as RectTransform;
		}
	}
}