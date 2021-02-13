using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour.Gameplay
{
	public class Cell : MonoBehaviour
	{
		[SerializeField] private Image m_fill = default;

		private LayoutElement m_layout = null;

		private void OnEnable()
		{
			Empty();
		}

		public void Empty()
		{
			m_fill.enabled = false;
		}

		public void Fill()
		{
			m_fill.enabled = true;
		}

		public void SetSize( float newSize )
		{
			m_layout.preferredHeight = m_layout.preferredWidth = newSize;
		}

		private void Awake()
		{
			m_layout = GetComponent<LayoutElement>();
		}
	}
}