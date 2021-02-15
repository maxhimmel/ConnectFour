using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour.Ui
{
	using Utility;

	[RequireComponent( typeof( Button ) )]
	public class LoadNextLevel : MonoBehaviour
	{
		private Button m_button = null;

		private void Start()
		{
			m_button.onClick.AddListener( SceneUtility.LoadNextScene );
		}

		private void Awake()
		{
			m_button = GetComponent<Button>();
		}
	}
}