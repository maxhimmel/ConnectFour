using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Gameplay.Users
{
	using Animation;

	public abstract class UserController : MonoBehaviour
	{
		public bool IsThinking { get; protected set; }
		public int Id { get; private set; }

		protected GameManager Game { get { return GameManager.Instance; } }

		[SerializeField, Range( 0, GameRules.k_playerCount - 1 )] private int m_userId = -1;

		[Header( "Animation" )]
		[SerializeField] protected ArmAnimator m_arm = default;
		[SerializeField] protected float m_hideArmDuration = 0.5f;

		protected Vector2 m_initialAnchoredPos;

		public void SetId( int id )
		{
			m_userId = id;
		}

		public virtual void StartTurn()
		{
			IsThinking = true;
		}

		public virtual void EndTurn()
		{
			IsThinking = false;

			m_arm.StartMoving( m_hideArmDuration, m_initialAnchoredPos );
		}

		protected virtual void Start()
		{
			m_initialAnchoredPos = m_arm.AnchoredPos;
			
			Game.OnWonEvent += OnWon_Internal;
		}

		private void OnWon_Internal()
		{
			if ( Game.CurrentPlayer != m_userId ) { return; }

			OnWon();
		}

		protected virtual void OnWon()
		{
			m_arm.StartCelebrating();
		}

		private void OnDestroy()
		{
			if ( GameManager.Exists )
			{
				Game.OnWonEvent -= OnWon_Internal;
			}
		}
	}
}