using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameObject EnemyPrefab;

	public GameBoard GameBoard;

	public void SpawnNewEnemy()
	{
		if (GameBoard.HasFreeSpawnPoints)
		{
			Instantiate(EnemyPrefab, GameBoard.SpawnEnemy(), Quaternion.identity);
		}
		else
		{
			Debug.Log("Tabuleiro cheio");
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			SpawnNewEnemy();
		}
	}
}
