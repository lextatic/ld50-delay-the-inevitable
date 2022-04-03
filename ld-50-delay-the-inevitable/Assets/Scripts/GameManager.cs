using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private EnemySpawner _enemySpawner;

	[SerializeField]
	private Attack _playerAttackController;

	[SerializeField]
	private InputManager _inputManager;

	private List<Enemy> _enemies = new List<Enemy>();

	private bool _gameOver;

	public event Action OnPlayerDefeated;

	private void Start()
	{
		_playerAttackController.OnAttackStartedExecuting += PlayerAttackController_OnAttackStartedExecuting;
		_playerAttackController.OnAttackFinishedExecuting += PlayerAttackController_OnAttackFinishedExecuting;
		_gameOver = false;
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

			for (int i = 0; i < 2; i++)
			{
				if (_enemySpawner.TrySpawnNewEnemy(out var newEnemy))
				{
					_enemies.Add(newEnemy);
					newEnemy.OnEnemyDestroyed += Enemy_OnEnemyDestroyed;
					newEnemy.OnPlayerDefeated += Enemy_OnPlayerDefeated;
				}
			}
			yield return new WaitWhile(() => _enemies.Any(enemy => enemy.IsMoving));

			_playerAttackController.enabled = true;
			_playerAttackController.ReactivateAttackGraphics();
			_inputManager.enabled = true;
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
