using System;
using UnityEngine;

public class AttackShape : MonoBehaviour
{
	public event Action<Transform> OnAddTarget;
	public event Action<Transform> OnRemoveTarget;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		OnAddTarget?.Invoke(collision.transform);
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		OnRemoveTarget?.Invoke(collision.transform);
	}
}
