using UnityEngine;

public class ActorView : MonoBehaviour
{
	public Animator Animator;

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
