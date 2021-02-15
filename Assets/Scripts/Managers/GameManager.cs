using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour
{
	using Utility;
	using Gameplay;
	using Gameplay.Users;

	public class GameManager : Singleton<GameManager>
	{
		public event System.Action<int> OnPlayerTurnChangedEvent;
		public event System.Action OnWonEvent
		{
			add { if ( m_gameoverHandler != null ) { m_gameoverHandler.OnWonEvent += value; } }
			remove { if ( m_gameoverHandler != null ) { m_gameoverHandler.OnWonEvent -= value; } }
		}
		public event System.Action OnDrawEvent
		{
			add { if ( m_gameoverHandler != null ) { m_gameoverHandler.OnDrawEvent += value; } }
			remove { if ( m_gameoverHandler != null ) { m_gameoverHandler.OnDrawEvent -= value; } }
		}

		public bool IsInitialized { get; private set; } = false;
		public int CurrentPlayer { get; private set; } = 0;
		public GameRules Rules { get { return m_rules; } }
		public GameGrid Grid { get { return m_grid; } }
		public Vector2Int PrevPlacedPiece { get; private set; } = Vector2Int.one * -1;

		[Header( "Game Manager" )]
		[SerializeField] private float m_startDelay = 0.5f;
		[SerializeField] private GameRules m_rules = new GameRules();

		[Space]
		[SerializeField] private Color[] m_playerColors = new Color[GameRules.k_playerCount] { Color.red, Color.blue };

		private GameGrid m_grid = null;
		private IGameoverHandler m_gameoverHandler = null;
		private UserController[] m_users = null;

		public Color GetPlayerColor( int player )
		{
			if ( m_playerColors == null || m_playerColors.Length <= 0 ) { return Color.magenta; }
			if ( player < 0 || player >= m_playerColors.Length ) { return Color.magenta; }

			return m_playerColors[player];
		}

		public void PlacePiece( int rowIndex, int columnIndex )
		{
			Cell cell = m_grid.GetCell( rowIndex, columnIndex );
			if ( cell != null )
			{
				cell.Fill( CurrentPlayer );
			}

			PrevPlacedPiece = GridHelper.RowColVector( rowIndex, columnIndex );

			if ( m_gameoverHandler.IsGameover( this ) )
			{
				Grid.SetInteractionActive( false );
			}
			else
			{
				StartCoroutine( CycleToNextTurn() );
			}
		}

		private IEnumerator CycleToNextTurn()
		{
			UserController currentUser = m_users[CurrentPlayer];
			currentUser.EndTurn();

			// Wait a frame for animations and EventSystem to resolve ...
			yield return null;

			CurrentPlayer = GetNextPlayer( CurrentPlayer );
			StartPlayerTurn( CurrentPlayer );
		}

		private int GetNextPlayer( int currentPlayer )
		{
			return ++currentPlayer % GameRules.k_playerCount;
		}

		private void StartPlayerTurn( int player )
		{
			OnPlayerTurnChangedEvent?.Invoke( player );

			UserController user = m_users[player];
			user.StartTurn();
		}

		private IEnumerator Start()
		{
			m_gameoverHandler.OnWonEvent += OnWon;
			m_gameoverHandler.OnDrawEvent += OnDraw;

			m_grid.SetInteractionActive( false );
			yield return new WaitForSeconds( m_startDelay );

			AssignPlayerIds();
			SetPlayerOrder();
			StartPlayerTurn( CurrentPlayer );

			IsInitialized = true;
		}

		private void OnWon()
		{
			EndLosersTurns();
		}

		private void EndLosersTurns()
		{
			for ( int idx = 0; idx < m_users.Length; ++idx )
			{
				if ( idx == CurrentPlayer ) { continue; }

				UserController loser = m_users[idx];
				loser.EndTurn();
			}
		}

		private void OnDraw()
		{
			EndAllTurns();
		}

		private void EndAllTurns()
		{
			for ( int idx = 0; idx < m_users.Length; ++idx )
			{
				UserController user = m_users[idx];
				user.EndTurn();
			}
		}

		private void AssignPlayerIds()
		{
			for ( int idx = 0; idx < m_users.Length; ++idx )
			{
				UserController user = m_users[idx];
				user.SetId( idx );
			}
		}

		private void SetPlayerOrder()
		{
			CurrentPlayer = m_rules.RandomPlayerOrder
				? Random.Range( 0, GameRules.k_playerCount )
				: 0;
		}

		protected override void Awake()
		{
			base.Awake();

			m_grid = GetComponentInChildren<GameGrid>();
			m_gameoverHandler = GetComponentInChildren<GameoverHandler>();
			m_users = GetComponentsInChildren<UserController>( false );
		}

		private void OnDestroy()
		{
			if ( m_gameoverHandler != null )
			{
				m_gameoverHandler.OnWonEvent -= OnWon;
				m_gameoverHandler.OnDrawEvent -= OnDraw;
			}
		}
	}
}