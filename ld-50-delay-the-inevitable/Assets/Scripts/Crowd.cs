using System.Collections.Generic;
using UnityEngine;

public class Crowd : MonoBehaviour
{
	public GameManager GameManager;

	public GameBoard GameBoard;

	public GameObject EnemyActor;

	private List<ActorView> _allActors = new List<ActorView>();

	private void Start()
	{
		GameManager.OnTurnPassed += GameManager_OnTurnPassed;
	}

	private void GameManager_OnTurnPassed(int turn)
	{
		Vector2 randomPosition;

		for (int i = 0; i < 100; i++)
		{
			do
			{
				randomPosition = new Vector2(Random.Range(-11f, 11f), Random.Range(-7.5f, 6.5f));
			}
			while (randomPosition.magnitude < GameBoard.BoardOutsideSize + 1);

			var enemy = Instantiate(EnemyActor, randomPosition, Quaternion.identity).GetComponentInChildren<ActorView>();

			enemy.SetWeapon();
			if (enemy.transform.position.x > 0)
			{
				enemy.transform.root.localScale = new Vector3(1, 1, 1);
			}

			_allActors.Add(enemy);
		}
	}
}
