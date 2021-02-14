using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour.Gameplay
{
	using Utility;
	using Animation;

	public class Cell : MonoBehaviour
	{
		public const int k_nonePlayer = -1;

		public Vector2Int Index { get; private set; } = Vector2Int.one * -1;
		public int PlayerOwner { get; private set; } = k_nonePlayer;

		private GameManager Game { get { return GameManager.Instance; } }

		[SerializeField] private Image m_fill = default;

		private LayoutElement m_layout = null;
		private Color m_emptyColor = Color.white;
		private ColorBlinker m_teaseBlinker = null;

		public void Config( float newSize, int row, int column )
		{
			m_layout.preferredHeight = m_layout.preferredWidth = newSize;
			Index = GridHelper.RowColVector( row, column );
		}

		public void Empty()
		{
			m_fill.enabled = false;
			PlayerOwner = k_nonePlayer;

			m_teaseBlinker.Stop();
			m_fill.color = m_emptyColor;
		}

		public void Fill( int player )
		{
			m_fill.enabled = true;
			PlayerOwner = player;

			m_teaseBlinker.Stop();
			
			m_fill.color = Game.GetPlayerColor( player );
		}

		public void PlayTeaserFill( float duration, Color startColor, Color endColor )
		{
			m_fill.enabled = true;
			m_teaseBlinker.Play( duration, startColor, endColor );
		}

		public void StopTeaserFill()
		{
			m_teaseBlinker.Stop();

			m_fill.enabled = false;
			m_fill.color = m_emptyColor;
		}

		private void Start()
		{
			Empty();
		}

		private void Awake()
		{
			m_layout = GetComponent<LayoutElement>();
			m_teaseBlinker = GetComponentInChildren<ColorBlinker>();

			m_emptyColor = m_fill.color;
		}
	}
}