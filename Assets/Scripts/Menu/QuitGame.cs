using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour.Ui
{
	[RequireComponent( typeof( Button ) )]
	public class QuitGame : MonoBehaviour
	{
		private Button m_button = null;

		private void Start()
		{
			m_button.onClick.AddListener( Quit );
		}

		private void Quit()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.ExitPlaymode();
#else
			Application.Quit();
#endif
		}

		private void Awake()
		{
			m_button = GetComponent<Button>();
		}
	}
}