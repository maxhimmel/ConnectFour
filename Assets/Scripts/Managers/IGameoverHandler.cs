using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Gameplay
{
	public interface IGameoverHandler
	{
		event System.Action OnWonEvent;
		event System.Action OnDrawEvent;

		/// <summary>
		/// Returns true if game is won or drawn.
		/// </summary>
		/// <returns></returns>
		bool IsGameover( GameManager game );
	}
}