using UnityEngine;

public class PlayerView : MonoBehaviour
{
	public Animator Animator;

	public Attack AttackComponent;

	public GameManager GameManager;

	public GameObject[] SheathedWeapons;
	public GameObject[] EquippedWeapons;

	public ParticleSystem BloodParticles;

	private void Start()
	{
		AttackComponent.OnWeaponChanged += AttackComponent_OnWeaponChanged;
		AttackComponent.OnMovingForAttack += AttackComponent_OnMovingForAttack;
		AttackComponent.OnMovingBack += AttackComponent_OnMovingBack;
		AttackComponent.OnAttackFinishedExecuting += AttackComponent_OnAttackFinishedExecuting;
		AttackComponent.OnAttackSwing += AttackComponent_OnAttackSwing;

		GameManager.OnPlayerDefeated += GameManager_OnPlayerDefeated;
	}

	private void GameManager_OnPlayerDefeated()
	{
		Animator.SetInteger("State", 5);

		BloodParticles.Emit(5);
	}

	private void AttackComponent_OnAttackSwing()
	{
		if (EquippedWeapons[1].activeInHierarchy)
		{
			Animator.SetInteger("State", 3);
		}
		else
		{
			Animator.SetInteger("State", 4);
		}
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
