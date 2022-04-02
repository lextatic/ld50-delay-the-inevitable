using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private EnemySpawner _enemySpawner;

	[SerializeField]
	private Attack _playerAttackController;

	private List<Enemy> _enemies = new List<Enemy>();

	private void Start()
	{
		_playerAttackController.OnAttackExecuted += PlayerAttackController_OnAttackExecuted;
	}

	private void PlayerAttackController_OnAttackExecuted()
	{
		foreach (var enemy in _enemies)
		{
			enemy.TakeTurn();
		}

		if (_enemySpawner.TrySpawnNewEnemy(out var newEnemy))
		{
			_enemies.Add(newEnemy);
			newEnemy.OnEnemyDestroyed += Enemy_OnEnemyDestroyed;
		}
	}

	private void Enemy_OnEnemyDestroyed(Enemy destroyedEnemy)
	{
		_enemies.Remove(destroyedEnemy);
		destroyedEnemy.OnEnemyDestroyed -= Enemy_OnEnemyDestroyed;
	}
}
