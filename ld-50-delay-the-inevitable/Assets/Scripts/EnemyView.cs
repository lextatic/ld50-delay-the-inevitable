using DG.Tweening;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
	public Enemy EnemyBehaviour;

	public Animator Animator;

	public GameObject Mark;

	public GameObject Warning;

	public SimpleAudioEvent WarningSound;

	public ParticleSystem BloodParticles;

	public bool FadeIn = true;

	private bool _attackReady = false;

	public void ActivateMark()
	{
		Mark.SetActive(true);
		Warning.SetActive(false);
	}

	public void DeactivateMark()
	{
		Mark.SetActive(false);
		if (_attackReady)
		{
			Warning.SetActive(true);
		}
	}

	private void Start()
	{
		EnemyBehaviour.OnAttackExecuted += EnemyBehaviour_OnAttackExecuted;
		EnemyBehaviour.OnAttackReady += EnemyBehaviour_OnAttackReady;

		EnemyBehaviour.OnMoving += EnemyBehaviour_OnMoving;
		EnemyBehaviour.OnIdle += EnemyBehaviour_OnIdle;
		EnemyBehaviour.OnEnemyDestroyed += EnemyBehaviour_OnDeath;

		if (FadeIn)
		{
			var allSpriteRenderers = GetComponentsInChildren<SpriteRenderer>();
			foreach (var spriteRenderer in allSpriteRenderers)
			{
				spriteRenderer.color = new Color(1, 1, 1, 0);
				spriteRenderer.DOFade(1, 0.5f);
			}
		}
	}

	private void EnemyBehaviour_OnDeath(Enemy enemy)
	{
		Animator.SetInteger("State", 5);

		BloodParticles.Emit(5);

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
		WarningSound.Play(Camera.main.GetComponent<AudioSource>());
		_attackReady = true;
		Warning.SetActive(true);
	}

	private void EnemyBehaviour_OnAttackExecuted()
	{
		_attackReady = false;
		Warning.SetActive(false);
		Animator.SetInteger("State", 3);
	}


}
