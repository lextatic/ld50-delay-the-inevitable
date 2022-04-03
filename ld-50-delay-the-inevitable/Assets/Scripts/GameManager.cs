using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[Serializable]
	private struct SpawnData
	{
		public int Sword;
		public int Hammer;
		public int Dagger;
	}

	[SerializeField]
	private EnemySpawner _enemySpawner;

	[SerializeField]
	private Attack _playerAttackController;

	[SerializeField]
	private InputManager _inputManager;

	[SerializeField]
	private GameBoard _gameBoard;

	public int TurnsToVictory = 20;

	[SerializeField]
	private SpawnData[] _spawnDataPerTurn;

	private int _turnsCount = 0;

	private List<Enemy> _enemies = new List<Enemy>();

	private bool _gameOver;

	public event Action OnPlayerDefeated;

	public event Action<int> OnTurnPassed;

	public void AllowRestart()
	{
		_inputManager.enabled = true;
		_inputManager.GameOver = true;
	}

	private void Start()
	{
		_playerAttackController.OnAttackStartedExecuting += PlayerAttackController_OnAttackStartedExecuting;
		_playerAttackController.OnAttackFinishedExecuting += PlayerAttackController_OnAttackFinishedExecuting;
		_gameOver = false;
		_turnsCount = 0;

		_enemies.AddRange(FindObjectsOfType<Enemy>());

		foreach (var enemy in _enemies)
		{
			enemy.OnEnemyDestroyed += Enemy_OnEnemyDestroyed;
			enemy.OnPlayerDefeated += Enemy_OnPlayerDefeated;
			_gameBoard.OcupySpot(enemy.GridPosition);
		}
	}

	private void PlayerAttackController_OnAttackStartedExecuting()
	{
		_playerAttackController.enabled = false;
		_inputManager.enabled = false;
	}

	private void PlayerAttackController_OnAttackFinishedExecuting()
	{
		StartCoroutine(MonsterTurnCoroutine());
	}

	private IEnumerator MonsterTurnCoroutine()
	{
		// Attack
		// Move
		// Spawn

		foreach (var enemy in _enemies)
		{
			enemy.AttackTurn();
		}
		yield return new WaitWhile(() => _enemies.Any(enemy => enemy.IsAttacking));

		if (!_gameOver)
		{
			foreach (var enemy in _enemies)
			{
				enemy.MoveTurn();
			}
			yield return new WaitWhile(() => _enemies.Any(enemy => enemy.IsMoving));

			SpawnTurnEnemies();

			yield return new WaitWhile(() => _enemies.Any(enemy => enemy.IsMoving));

			_playerAttackController.enabled = true;
			_playerAttackController.ReactivateAttackGraphics();
			_inputManager.enabled = true;

			_turnsCount++;

			OnTurnPassed?.Invoke(_turnsCount);

			if (_turnsCount >= TurnsToVictory)
			{
				Debug.Log("Victory!");
			}
		}
	}
	private void SpawnTurnEnemies()
	{
		int spawnDataIndex = _turnsCount < _spawnDataPerTurn.Length ? _turnsCount : _spawnDataPerTurn.Length - 1;

		SpawnMultipleEnemies(_spawnDataPerTurn[spawnDataIndex].Sword, EnemyType.Sword);
		SpawnMultipleEnemies(_spawnDataPerTurn[spawnDataIndex].Hammer, EnemyType.Hammer);
		SpawnMultipleEnemies(_spawnDataPerTurn[spawnDataIndex].Dagger, EnemyType.Dagger);
	}

	private void SpawnMultipleEnemies(int number, EnemyType enemyType)
	{
		for (int i = 0; i < number; i++)
		{
			SpawnEnemy(enemyType);
		}
	}

	private void SpawnEnemy(EnemyType enemyType)
	{
		if (_enemySpawner.TrySpawnNewEnemy(out var newEnemy, enemyType))
		{
			_enemies.Add(newEnemy);
			newEnemy.OnEnemyDestroyed += Enemy_OnEnemyDestroyed;
			newEnemy.OnPlayerDefeated += Enemy_OnPlayerDefeated;
		}
	}

	private void Enemy_OnPlayerDefeated()
	{
		if (!_gameOver)
		{
			_playerAttackController.enabled = false;
			_inputManager.enabled = false;
			OnPlayerDefeated?.Invoke();
			_gameOver = true;
		}
	}

	private void Enemy_OnEnemyDestroyed(Enemy destroyedEnemy)
	{
		_enemies.Remove(destroyedEnemy);
		destroyedEnemy.OnEnemyDestroyed -= Enemy_OnEnemyDestroyed;
		destroyedEnemy.OnPlayerDefeated -= Enemy_OnPlayerDefeated;
	}
}
