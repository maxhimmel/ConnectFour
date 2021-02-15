using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Gameplay.Users
{
	public abstract class UserController : MonoBehaviour
	{
		public bool IsThinking { get; protected set; }

		protected GameManager Game { get { return GameManager.Instance; } }

		[Header( "Animation" )]
		[SerializeField] protected ArmAnimator m_arm = default;
		[SerializeField] protected float m_hideArmDuration = 0.5f;

		protected Vector2 m_initialAnchoredPos;

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
		}
	}
}