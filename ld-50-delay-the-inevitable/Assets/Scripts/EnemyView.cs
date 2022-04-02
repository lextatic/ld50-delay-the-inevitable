using UnityEngine;

public class EnemyView : MonoBehaviour
{
	public Enemy EnemyBehaviour;

	public GameObject Mark;

	public GameObject Warning;

	private void Start()
	{
		EnemyBehaviour.OnAttackExecuted += EnemyBehaviour_OnAttackExecuted;
		EnemyBehaviour.OnAttackReady += EnemyBehaviour_OnAttackReady;
	}

	private void EnemyBehaviour_OnAttackReady()
	{
		Warning.SetActive(true);
	}

	private void EnemyBehaviour_OnAttackExecuted()
	{
		Warning.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Mark.SetActive(true);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Mark.SetActive(false);
	}
}
