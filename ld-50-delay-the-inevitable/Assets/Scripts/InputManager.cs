using UnityEngine;

public class InputManager : MonoBehaviour
{
	public Attack Attack;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Attack.SelectAttack(0);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			Attack.SelectAttack(1);
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			Attack.SelectAttack(2);
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			Attack.ExecuteAttack();
		}
	}
}
