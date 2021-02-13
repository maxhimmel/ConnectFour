using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour.Gameplay
{
	public class Column : MonoBehaviour
	{
		private VerticalLayoutGroup m_layout = null;

		private void Awake()
		{
			m_layout = GetComponent<VerticalLayoutGroup>();
		}
	}
}