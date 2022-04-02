using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public Enemy EnemyPrefab;

	public GameBoard GameBoard;

	public bool TrySpawnNewEnemy(out Enemy enemy)
	{
		enemy = null;

		if (GameBoard.HasFreeSpawnPoints)
		{
			var position = GameBoard.SpawnEnemy();
			enemy = Instantiate(EnemyPrefab);
			enemy.GridPosition = position;

			return true;
		}

		return false;
	}
}
