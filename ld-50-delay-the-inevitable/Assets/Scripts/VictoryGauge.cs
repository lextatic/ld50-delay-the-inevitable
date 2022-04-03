using DG.Tweening;
using UnityEngine;

public class VictoryGauge : MonoBehaviour
{
	public GameManager GameManager;

	public RectTransform FillBar;

	public RectTransform BaseBar;

	private void Start()
	{
		GameManager.OnTurnPassed += GameManager_OnTurnPassed;
		FillBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
	}

	private void GameManager_OnTurnPassed(int currentTurn)
	{
		var size = ((float)currentTurn / GameManager.TurnsToVictory) * BaseBar.rect.width;

		if (size > BaseBar.rect.width) return;

		FillBar.DOSizeDelta(new Vector2(size, FillBar.rect.height), 1f);
	}
}
