using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour.Gameplay
{
	using Animation;

	public class TurnDisplayManager : MonoBehaviour
	{
		private GameManager Game { get { return GameManager.Instance; } }

		private TurnDisplay[] m_displays = null;

		private void Start()
		{
			Game.OnWonEvent += OnWon;
			Game.OnPlayerTurnChangedEvent += OnPlayerTurnChanged;

			for ( int pdx = 0; pdx < m_displays.Length; ++pdx )
			{
				TurnDisplay display = m_displays[pdx];
				display.SetColor( pdx );
			}
		}

		private void OnWon()
		{
			TurnDisplay display = m_displays[Game.CurrentPlayer];
			StartCoroutine( WaitForShowHideCompletion( display, () =>
			{
				if ( display == null ) { return; }

				SineWaveRotator displayRotator = display.GetComponent<SineWaveRotator>();
				if ( displayRotator != null )
				{
					displayRotator.Play();
				}
			} ) );
		}

		private IEnumerator WaitForShowHideCompletion( TurnDisplay display, System.Action callback )
		{
			while ( display != null && display.IsShowingOrHiding ) { yield return null; }

			callback?.Invoke();
		}

		private void OnPlayerTurnChanged( int player )
		{
			TurnDisplay currentDisplay = m_displays[player];
			currentDisplay.Show();

			int prevPlayer = (player + 1) % GameRules.k_playerCount;
			TurnDisplay prevDisplay = m_displays[prevPlayer];
			prevDisplay.Hide();
		}

		private void Awake()
		{
			m_displays = GetComponentsInChildren<TurnDisplay>();
		}

		private void OnDestroy()
		{
			if ( GameManager.Exists )
			{
				Game.OnWonEvent -= OnWon;
				Game.OnPlayerTurnChangedEvent -= OnPlayerTurnChanged;
			}
		}
	}
}