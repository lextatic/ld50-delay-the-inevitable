using System;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public AttackShape[] AttackShapes;

	public float RotationSpeed = 0.1f;

	private List<Transform> _currentTargets = new List<Transform>();

	public event Action OnAttackExecuted;

	public void SelectAttack(int index)
	{
		Debug.Assert(index <= AttackShapes.Length);

		_currentTargets.Clear();

		for (int i = 0; i < AttackShapes.Length; i++)
		{
			AttackShapes[i].gameObject.SetActive(index == i);
		}
	}

	public void ExecuteAttack()
	{
		OnAttackExecuted?.Invoke();

		for (int i = _currentTargets.Count - 1; i >= 0; i--)
		{
			Destroy(_currentTargets[i].gameObject);
		}
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

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log(collision.name);
	}
}
