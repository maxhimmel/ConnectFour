using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ConnectFour.Gameplay
{
	using Animation;

	public class GameoverDisplay : MonoBehaviour
	{
		private GameManager Game { get { return GameManager.Instance; } }

		[Header( "Messages" )]
		[SerializeField] private float m_showDuration = 0.35f;
		[SerializeField] private string m_winMessage = "Winner!";
		[SerializeField] private string m_drawMessage = "Draw ...";

		[Header( "VFX" )]
		[SerializeField] private Color m_bgColor = Color.white;

		[Header( "References" )]
		[SerializeField] private ParticleSystem m_celebrationVfx = default;
		[SerializeField] private CanvasGroupFader m_displayFader = default;

		[Space]
		[SerializeField] private TMP_Text m_gameoverMessageElement = default;
		[SerializeField] private SineWaveRotator m_messageRotator = default;

		private void Start()
		{
			Game.OnDrawEvent += OnDraw;
			Game.OnWonEvent += OnWon;

			m_displayFader.FadeOut( 0 );
		}

		private void OnDraw()
		{
			ShowMessage( m_drawMessage );
		}

		private void ShowMessage( string message )
		{
			m_displayFader.FadeIn( m_showDuration );
			m_gameoverMessageElement.text = message;
		}

		private void OnWon()
		{
			SetVfxColors( Game.GetPlayerColor( Game.CurrentPlayer ) );
			ShowMessage( m_winMessage );

			m_messageRotator.Play();
			m_celebrationVfx.Play( true );
		}

		private void SetVfxColors( Color color )
		{
			m_gameoverMessageElement.color = color;

			SetParticleColor( m_celebrationVfx, color );
		}

		private void SetParticleColor( ParticleSystem particle, Color color )
		{
			ParticleSystem.MainModule particleModule = particle.main;

			particleModule.startColor = new ParticleSystem.MinMaxGradient( color, m_bgColor );
		}

		private void OnDestroy()
		{
			if ( GameManager.Exists )
			{
				Game.OnDrawEvent -= OnDraw;
				Game.OnWonEvent -= OnWon;
			}
		}
	}
}