using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Utility
{
	public static class GridHelper
	{
		public static int Row( this Vector2Int self )
		{
			return self.y;
		}

		public static int Col( this Vector2Int self )
		{
			return self.x;
		}

		public static Vector2Int RowColVector( int row, int column )
		{
			return new Vector2Int( column, row );
		}
	}
}