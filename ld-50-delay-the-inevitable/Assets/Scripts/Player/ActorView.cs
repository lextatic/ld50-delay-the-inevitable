using UnityEngine;

public class ActorView : MonoBehaviour
{
	public Animator Animator;

	public GameObject[] SheathedWeapons;
	public GameObject[] EquippedWeapons;

	private void Idle()
	{
		Animator.SetInteger("State", 0);
	}

	private void Run()
	{
		Animator.SetInteger("State", 1);
	}

	private void RunAttack()
	{
		Animator.SetInteger("State", 2);
	}

	private void ChangeWeapon(int weaponIndex)
	{
		for (int i = 0; i < SheathedWeapons.Length; i++)
		{
			SheathedWeapons[i].SetActive(i != weaponIndex);
			EquippedWeapons[i].SetActive(i == weaponIndex);
		}
	}
}
