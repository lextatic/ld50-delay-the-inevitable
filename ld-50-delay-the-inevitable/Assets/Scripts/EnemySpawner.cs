using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public Enemy EnemyPrefab;

	public GameBoard GameBoard;

	public void SpawnNewEnemy()
	{
		if (GameBoard.HasFreeSpawnPoints)
		{
			var position = GameBoard.SpawnEnemy();
			var enemy = Instantiate(EnemyPrefab);
			enemy.GridPosition = position;
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
