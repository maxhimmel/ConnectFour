using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Gameplay.Users
{
	public class PlayerController : UserController
	{
		public override void StartTurn()
		{
			base.StartTurn();

			Game.Grid.SetInteractionActive( true );
		}

		public override void EndTurn()
		{
			base.EndTurn();

			Game.Grid.SetInteractionActive( false );
		}
	}
}