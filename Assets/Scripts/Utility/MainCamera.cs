using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Utility
{
	[RequireComponent( typeof( Camera ) )]
	public class MainCamera : Singleton<MainCamera>
	{
		public static Camera Cam { get { return Instance.m_mainCamera; } }

		private Camera m_mainCamera = null;

		protected override void Awake()
		{
			base.Awake();

			m_mainCamera = GetComponent<Camera>();
		}
	}
}