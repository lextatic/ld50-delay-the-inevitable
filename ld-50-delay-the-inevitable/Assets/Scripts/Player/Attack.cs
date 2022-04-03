using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public AttackShape[] AttackShapes;

	public float RotationSpeed = 0.1f;

	public Color NormalColor;
	public Color ValidTargetColor;
	public Color ErrorTargetColor;

	public SimpleAudioEvent SlashSound;
	public SimpleAudioEvent AttackAction;
	public SimpleAudioEvent AttackActionError;
	public SimpleAudioEvent BackToAction;

	private List<Transform> _currentTargets = new List<Transform>();
	private SpriteRenderer[] _attackShapeRenderers;

	private int _currentAttackIndex;

	public event Action<int> OnWeaponChanged;
	public event Action OnMovingForAttack;
	public event Action OnMovingBack;
	public event Action OnAttackSwing;
	public event Action OnAttackStartedExecuting;
	public event Action OnAttackFinishedExecuting;

	public Transform Player;

	public void SelectAttack(int index)
	{
		Debug.Assert(index <= AttackShapes.Length);

		_currentAttackIndex = index;

		_currentTargets.Clear();

		for (int i = 0; i < AttackShapes.Length; i++)
		{
			AttackShapes[i].gameObject.SetActive(index == i);
		}

		OnWeaponChanged?.Invoke(index);
	}

	public void ExecuteAttack()
	{
		OnAttackStartedExecuting?.Invoke();

		StartCoroutine(AttackAnimationSequence());
	}

	public void ReactivateAttackGraphics()
	{
		if (_currentTargets.Count > 0)
		{
			_attackShapeRenderers[_currentAttackIndex].color = new Color(ValidTargetColor.r, ValidTargetColor.g, ValidTargetColor.b, 0); ;
		}
		else
		{
			_attackShapeRenderers[_currentAttackIndex].color = new Color(NormalColor.r, NormalColor.g, NormalColor.b, 0); ;
		}

		BackToAction.Play(Camera.main.GetComponent<AudioSource>());

		//GetComponentInChildren<SpriteRenderer>().DOFade(0.4f, 0f);
		GetComponentInChildren<SpriteRenderer>().DOFade(0.4f, 0.1f).SetLoops(3, LoopType.Yoyo).SetEase(Ease.Linear);
	}

	private IEnumerator AttackAnimationSequence()
	{
		var renderer = _attackShapeRenderers[_currentAttackIndex];

		if (_currentTargets.Count <= 0)
		{
			AttackActionError.Play(Camera.main.GetComponent<AudioSource>());
			renderer.color = ErrorTargetColor;
		}
		else
		{
			AttackAction.Play(Camera.main.GetComponent<AudioSource>());
		}

		renderer.DOFade(0, 0.1f).SetLoops(7, LoopType.Yoyo).SetEase(Ease.Linear);

		var attacked = false;
		for (int i = _currentTargets.Count - 1; i >= 0; i--)
		{
			OnMovingForAttack?.Invoke();
			// Anim attack (doTween)
			var offset = Vector3.zero;
			if (Player.position.x - _currentTargets[i].position.x < 0)
			{
				Player.localScale = new Vector3(-1, 1, 1);
				offset = Vector3.left;
			}
			else
			{
				offset = Vector3.right;
				Player.localScale = new Vector3(1, 1, 1);
			}
			Player.DOMove(_currentTargets[i].position + offset, 0.7f);
			yield return new WaitForSeconds(0.7f);
			OnAttackSwing?.Invoke();
			SlashSound.Play(Camera.main.GetComponent<AudioSource>());
			yield return new WaitForSeconds(0.60f);
			Debug.Log($"_currentTargets.Count: {_currentTargets.Count} - i: {i}");
			_currentTargets[i].GetComponent<Enemy>().Kill();
			yield return new WaitForSeconds(0.15f);
			attacked = true;
		}

		// Anim go back (doTween)
		if (attacked)
		{
			if (Player.position.x < 0)
			{
				Player.localScale = new Vector3(-1, 1, 1);
			}
			else
			{
				Player.localScale = new Vector3(1, 1, 1);
			}

			OnMovingBack?.Invoke();
		}

		Player.DOMove(Vector3.zero, 0.7f);
		yield return new WaitForSeconds(0.7f);

		OnAttackFinishedExecuting?.Invoke();
	}

	private void Start()
	{
		foreach (var attackShape in AttackShapes)
		{
			attackShape.OnAddTarget += AttackShape_OnAddTarget;
			attackShape.OnRemoveTarget += AttackShape_OnRemoveTarget;
		}

		_currentAttackIndex = 1;

		_attackShapeRenderers = new SpriteRenderer[AttackShapes.Length];
		for (int i = 0; i < AttackShapes.Length; i++)
		{
			_attackShapeRenderers[i] = AttackShapes[i].GetComponent<SpriteRenderer>();
		}
	}

	private void AttackShape_OnAddTarget(Transform newTarget)
	{
		if (_currentTargets.Count == 0)
		{
			var renderer = _attackShapeRenderers[_currentAttackIndex];
			if (renderer.color.a == 0.0f)
			{
				renderer.color = new Color(ValidTargetColor.r, ValidTargetColor.g, ValidTargetColor.b, 0);
			}
			else
			{
				renderer.color = ValidTargetColor;
			}
			//GetComponentInChildren<SpriteRenderer>().color = ValidTargetColor;
		}

		_currentTargets.Add(newTarget);
	}

	private void AttackShape_OnRemoveTarget(Transform targetRemoved)
	{
		_currentTargets.Remove(targetRemoved);

		if (_currentTargets.Count == 0)
		{
			var renderer = _attackShapeRenderers[_currentAttackIndex];
			if (renderer.color.a == 0.0f)
			{
				renderer.color = new Color(NormalColor.r, NormalColor.g, NormalColor.b, 0);
			}
			else
			{
				renderer.color = NormalColor;
			}

			//GetComponentInChildren<SpriteRenderer>().color = NormalColor;
		}
	}

	private void Update()
	{
		transform.Rotate(Vector3.forward, -RotationSpeed * Time.deltaTime * 360f);
	}
}
