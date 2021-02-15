using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ConnectFour.Utility
{
	public static class SceneUtility
	{
		public static void LoadNextScene()
		{
			Scene current = SceneManager.GetActiveScene();

			int nextLevel = (current.buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
			SceneManager.LoadScene( nextLevel, LoadSceneMode.Single );
		}

		public static void ReloadActiveScene()
		{
			Scene current = SceneManager.GetActiveScene();
			SceneManager.LoadScene( current.buildIndex, LoadSceneMode.Single );
		}
	}
}