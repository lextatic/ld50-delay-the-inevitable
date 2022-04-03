using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
	public ActorView PlayerActor;
	public ActorView Actor1;
	public ActorView Actor2;

	public ActorView[] EnemyActor;

	private void Update()
	{
		if (Input.anyKey)
		{
			LoadGame();
		}
	}

	public void EnemyRun()
	{
		foreach (var enemy in EnemyActor)
		{
			enemy.Run();
		}
	}

	public void EnemyIdle()
	{
		foreach (var enemy in EnemyActor)
		{
			enemy.Idle();
		}
	}

	public void PlayerRun()
	{
		PlayerActor.Run();
	}

	public void PlayerIdle()
	{
		PlayerActor.Idle();
	}

	public void Actor1Run()
	{
		Actor1.Run();
	}

	public void Actor1Idle()
	{
		Actor1.Idle();
	}

	public void Actor2Run()
	{
		Actor2.Run();
	}

	public void Actor2Idle()
	{
		Actor2.Idle();
	}

	public void LoadGame()
	{
		SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
	}
}
