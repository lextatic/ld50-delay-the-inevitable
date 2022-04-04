using UnityEngine;

public class ActorView : MonoBehaviour
{
	public Animator Animator;

	public Sprite Dagger;
	public Sprite Hammer;

	public SpriteRenderer Renderer;

	public void SetWeapon()
	{
		int randomWeapon = Random.Range(0, 3);

		if (randomWeapon == 1)
		{
			Renderer.sprite = Dagger;
		}

		else if (randomWeapon == 2)
		{
			Renderer.sprite = Hammer;
		}
	}

	public void Idle()
	{
		Animator.SetInteger("State", 0);
	}

	public void Run()
	{
		Animator.SetInteger("State", 1);
	}

	public void RunAttack()
	{
		Animator.SetInteger("State", 2);
	}
}
