using UnityEngine;

public class PlayerView : MonoBehaviour
{
	public Animator Animator;

	public Attack AttackComponent;

	public GameObject[] SheathedWeapons;
	public GameObject[] EquippedWeapons;

	private void Start()
	{
		AttackComponent.OnWeaponChanged += AttackComponent_OnWeaponChanged;
		AttackComponent.OnMovingForAttack += AttackComponent_OnMovingForAttack;
		AttackComponent.OnMovingBack += AttackComponent_OnMovingBack;
		AttackComponent.OnAttackFinishedExecuting += AttackComponent_OnAttackFinishedExecuting;
		AttackComponent.OnAttackSwing += AttackComponent_OnAttackSwing;
	}

	private void AttackComponent_OnAttackSwing()
	{
		Animator.SetInteger("State", 3);
	}

	private void AttackComponent_OnAttackFinishedExecuting()
	{
		Animator.SetInteger("State", 0);
	}

	private void AttackComponent_OnMovingBack()
	{
		Animator.SetInteger("State", 1);
	}

	private void AttackComponent_OnMovingForAttack()
	{
		Animator.SetInteger("State", 2);
	}

	private void AttackComponent_OnWeaponChanged(int weaponIndex)
	{
		for (int i = 0; i < SheathedWeapons.Length; i++)
		{
			SheathedWeapons[i].SetActive(i != weaponIndex);
			EquippedWeapons[i].SetActive(i == weaponIndex);
		}
	}
}