using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
	private void Update()
	{
		if (Input.anyKey)
		{
			SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
		}
	}
}
