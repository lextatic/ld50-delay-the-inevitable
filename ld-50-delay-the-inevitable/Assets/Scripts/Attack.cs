using UnityEngine;

public class Attack : MonoBehaviour
{
	public Transform[] AttackShapes;

	public float RotationSpeed = 0.1f;

	public void SelectAttack(int index)
	{
		Debug.Assert(index <= AttackShapes.Length);

		for (int i = 0; i < AttackShapes.Length; i++)
		{
			AttackShapes[i].gameObject.SetActive(index == i);
		}
	}

	private void Update()
	{
		transform.Rotate(Vector3.forward, -RotationSpeed * Time.deltaTime * 360f);

		// Temp input
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			SelectAttack(0);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			SelectAttack(1);
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			SelectAttack(2);
		}
	}
}
