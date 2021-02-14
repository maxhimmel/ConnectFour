using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour
{
	[System.Serializable]
	public class GameRules
	{
		public const int k_playerCount = 2;

		public int WinningConnectCount { get { return m_winningConnectCount; } }
		public int RowCount { get { return m_rowCount; } }
		public int ColumnCount { get { return m_columnCount; } }
		public int TotalCellCount { get { return m_rowCount * m_columnCount; } }

		[SerializeField, Range( 2, 9 )] private int m_winningConnectCount = 4;

		[Space]
		[SerializeField, Range( 3, 13 )] private int m_rowCount = 6;
		[SerializeField, Range( 3, 13 )] private int m_columnCount = 7;
	}
}