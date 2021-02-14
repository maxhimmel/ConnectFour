using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Gameplay.Users
{
	public abstract class UserController : MonoBehaviour
	{
		public bool IsThinking { get; protected set; }

		protected GameManager Game { get { return GameManager.Instance; } }

		public virtual void StartTurn()
		{
			IsThinking = true;
		}

		public virtual void EndTurn()
		{
			IsThinking = false;
		}
	}
}