using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Attack : MonoBehaviour
{
	public AttackShape[] AttackShapes;

	public float RotationSpeed = 0.1f;

	private List<Transform> _currentTargets = new List<Transform>();

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
		GetComponentInChildren<SpriteShapeRenderer>().enabled = true;
	}

	private IEnumerator AttackAnimationSequence()
	{
		// Blink (horrible solution btw)
		var renderer = GetComponentInChildren<SpriteShapeRenderer>();
		renderer.enabled = false;
		yield return new WaitForSeconds(0.1f);
		renderer.enabled = true;
		yield return new WaitForSeconds(0.1f);
		renderer.enabled = false;
		yield return new WaitForSeconds(0.1f);
		renderer.enabled = true;
		yield return new WaitForSeconds(0.1f);
		renderer.enabled = false;
		yield return new WaitForSeconds(0.1f);
		renderer.enabled = true;
		yield return new WaitForSeconds(0.1f);
		renderer.enabled = false;

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
			yield return new WaitForSeconds(0.60f);
			Destroy(_currentTargets[i].gameObject);
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
	}

	private void AttackShape_OnAddTarget(Transform newTarget)
	{
		_currentTargets.Add(newTarget);
	}

	private void AttackShape_OnRemoveTarget(Transform targetRemoved)
	{
		_currentTargets.Remove(targetRemoved);
	}

	private void Update()
	{
		transform.Rotate(Vector3.forward, -RotationSpeed * Time.deltaTime * 360f);
	}

	//private void OnTriggerEnter2D(Collider2D collision)
	//{
	//	Debug.Log(collision.name);
	//}
}