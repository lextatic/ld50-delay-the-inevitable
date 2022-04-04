using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
	public ActorView PlayerActor;
	public ActorView Actor1;
	public ActorView Actor2;

	public ActorView[] EnemyActor;

	public Image TitleBackground;

	public TextMeshProUGUI TitleText;

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

	public void AnimateTitle()
	{
		TitleBackground.DOFade(1f, .2f).OnComplete(() =>
		{
			TitleText.DOFade(1f, 1f).SetLoops(2, LoopType.Yoyo);
			TitleText.rectTransform.DOLocalMoveX(-200, 2f);

			TitleBackground.DOFade(0f, .2f).SetDelay(2f);
		});
	}

	public void LoadGame()
	{
		SceneManager.LoadSceneAsync(1);
	}
}
