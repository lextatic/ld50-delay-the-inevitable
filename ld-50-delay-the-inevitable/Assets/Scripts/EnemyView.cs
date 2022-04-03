using DG.Tweening;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
	public Enemy EnemyBehaviour;

	public Animator Animator;

	public GameObject Mark;

	public GameObject Warning;

	private void Start()
	{
		EnemyBehaviour.OnAttackExecuted += EnemyBehaviour_OnAttackExecuted;
		EnemyBehaviour.OnAttackReady += EnemyBehaviour_OnAttackReady;

		EnemyBehaviour.OnMoving += EnemyBehaviour_OnMoving;
		EnemyBehaviour.OnIdle += EnemyBehaviour_OnIdle;
		EnemyBehaviour.OnEnemyDestroyed += EnemyBehaviour_OnDeath;

		var allSpriteRenderers = GetComponentsInChildren<SpriteRenderer>();

		foreach (var spriteRenderer in allSpriteRenderers)
		{
			spriteRenderer.color = new Color(1, 1, 1, 0);
			spriteRenderer.DOFade(1, 0.5f);
		}
	}

	private void EnemyBehaviour_OnDeath(Enemy enemy)
	{
		Animator.SetInteger("State", 5);

		var allSpriteRenderers = GetComponentsInChildren<SpriteRenderer>();

		foreach (var spriteRenderer in allSpriteRenderers)
		{
			spriteRenderer.DOFade(0, 0.2f).SetDelay(1f);
		}
	}

	private void EnemyBehaviour_OnIdle()
	{
		Animator.SetInteger("State", 0);
	}

	private void EnemyBehaviour_OnMoving()
	{
		Animator.SetInteger("State", 1);
	}

	private void EnemyBehaviour_OnAttackReady()
	{
		Warning.SetActive(true);
	}

	private void EnemyBehaviour_OnAttackExecuted()
	{
		Warning.SetActive(false);
		Animator.SetInteger("State", 3);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Mark.SetActive(true);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Mark.SetActive(false);
	}
}
