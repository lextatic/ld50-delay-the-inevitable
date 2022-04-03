using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverView : MonoBehaviour
{
	public GameManager GameManager;

	public Image GameOverPanel;

	public Image[] LetterboxImages;

	public TextMeshProUGUI GameOverText;
	public TextMeshProUGUI GameOverDescriptionText;

	[Header("Fade Out")]
	public Image ButtonAttack;
	public Image ButtonWeapon1;
	public Image ButtonWeapon2;
	public Image ButtonWeapon3;

	public GameObject Bar;

	private void Start()
	{
		GameManager.OnPlayerDefeated += GameManager_OnPlayerDefeated;
	}

	private void GameManager_OnPlayerDefeated()
	{
		StartCoroutine(PlayerDefeatedCoroutine());
	}

	private IEnumerator PlayerDefeatedCoroutine()
	{
		yield return new WaitForSeconds(1f);

		foreach (var image in LetterboxImages)
		{
			image.DOFade(1f, 2f);
		}

		GameOverPanel.DOFade(0.5f, 2f);
		GameOverText.DOFade(1f, 2f);
		GameOverDescriptionText.DOFade(1f, 2f);

		ButtonAttack.DOFade(0f, 2f);
		ButtonWeapon1.DOFade(0f, 2f);
		ButtonWeapon2.DOFade(0f, 2f);
		ButtonWeapon3.DOFade(0f, 2f);

		var barImages = Bar.GetComponentsInChildren<Image>();

		foreach (var image in barImages)
		{
			image.DOFade(0f, 2f);
		}

		if (GameManager.CurrentTurn >= GameManager.TurnsToVictory)
		{
			GameOverText.text = "Victory?";
			GameOverDescriptionText.text = "You did it, at least your friends are safe.";
		}
		else
		{
			GameOverText.text = "Game Over";
			GameOverDescriptionText.text = "Your friends didn't make it.";
		}
	}
}
