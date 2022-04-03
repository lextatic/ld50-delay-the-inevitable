using UnityEngine;

public enum EnemyType
{
	Sword,
	Hammer,
	Dagger
}

public class EnemySpawner : MonoBehaviour
{
	public Enemy SwordEnemyPrefab;
	public Enemy HammerEnemyPrefab;
	public Enemy DaggerEnemyPrefab;

	public GameBoard GameBoard;

	public bool TrySpawnNewEnemy(out Enemy enemy, EnemyType enemyType)
	{
		enemy = null;


		if (GameBoard.HasFreeSpawnPoints)
		{
			Enemy enemyPrefab = null;

			switch (enemyType)
			{
				default:
				case EnemyType.Sword:
					enemyPrefab = SwordEnemyPrefab;
					break;

				case EnemyType.Hammer:
					enemyPrefab = HammerEnemyPrefab;
					break;

				case EnemyType.Dagger:
					enemyPrefab = DaggerEnemyPrefab;
					break;
			}

			var position = GameBoard.SpawnEnemy();
			enemy = Instantiate(enemyPrefab);
			enemy.transform.position = GameBoard.GetWorldPosition(position.x, position.y + 1);
			enemy.GridPosition = position;

			return true;
		}

		return false;
	}
}
