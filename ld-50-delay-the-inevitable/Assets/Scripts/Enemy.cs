using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Vector2Int GridPosition;

	public float MoveForwardChance = 0.6f;
	public float MoveSidewaysChance = 0.3f;

	public int TurnsToAttack = 8;

	[Tooltip("x = min , y = max")]
	public Vector2Int AttackRange = new Vector2Int(0, 1);

	private int _currentTurnsToAttack;

	private GameBoard _gameBoard;

	private bool _isAttackReady;

	public event Action OnAttackReady;

	public event Action OnAttackExecuted;

	public event Action OnPlayerDefeated;

	public event Action OnMoving;

	public event Action OnIdle;

	//public event Action OnDeath;

	public event Action<Enemy> OnEnemyDestroyed;

	public bool IsMoving { get; private set; } = true;

	public bool IsAttacking { get; private set; }

	public void AttackTurn()
	{
		_currentTurnsToAttack--;

		if (_currentTurnsToAttack <= 1 && GridPosition.y >= AttackRange.x && GridPosition.y <= AttackRange.y)
		{
			if (_isAttackReady)
			{
				IsAttacking = true;

				OnMoving?.Invoke();

				var offset = Vector3.zero;
				if (transform.position.x < 0)
				{
					transform.localScale = new Vector3(-1, 1, 1);
					offset = Vector3.left;
				}
				else
				{
					offset = Vector3.right;
					transform.localScale = new Vector3(1, 1, 1);
				}

				transform.DOMove(Vector3.zero + offset, 0.7f).OnComplete(() =>
				{
					StartCoroutine(AttackCoroutine());
				});
				return;
			}
			else
			{
				_isAttackReady = true;
				OnAttackReady?.Invoke();
			}
		}
	}

	public void MoveTurn()
	{
		IsMoving = true;
		AIMove();
	}

	public void Kill()
	{
		_gameBoard.ReleasePosition(GridPosition);
		IsMoving = false;
		OnEnemyDestroyed?.Invoke(this);

		StartCoroutine(DeathCoroutine());

		//OnDeath?.Invoke();
		//Destroy(_currentTargets[i].gameObject);
	}

	private IEnumerator DeathCoroutine()
	{
		yield return new WaitForSeconds(1.2f);
		Destroy(gameObject);
	}

	private IEnumerator AttackCoroutine()
	{
		//transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);

		// Animar ataque, dar delay
		OnAttackExecuted?.Invoke();
		yield return new WaitForSeconds(0.60f);

		OnPlayerDefeated?.Invoke();

		yield return new WaitForSeconds(0.15f);
		transform.DOMove(_gameBoard.GetWorldPosition(GridPosition.x, GridPosition.y), 0.5f).OnComplete(() =>
		{
			IsAttacking = false;
			OnIdle?.Invoke();
		});
	}

	private void AIMove()
	{
		var randomChance = UnityEngine.Random.value;

		// 60% forward, 30% side, 10% back
		if (randomChance <= MoveForwardChance)
		{
			MoveSafe(Direction.In);
		}
		else if (randomChance <= MoveForwardChance + MoveSidewaysChance / 2)
		{
			MoveSafe(Direction.Right);
		}
		else if (randomChance <= MoveForwardChance + MoveSidewaysChance)
		{
			MoveSafe(Direction.Left);
		}
		else
		{
			MoveSafe(Direction.Out);
		}
	}

	private void MoveSafe(Direction direction)
	{
		if (_gameBoard.IsValidPosition(GridPosition, direction))
		{
			Move(direction);
		}
		else
		{
			IsMoving = false;
		}
	}

	private void Awake()
	{
		_gameBoard = GameObject.FindWithTag("Managers").GetComponent<GameBoard>();
	}

	private void Start()
	{
		UpdateWorldPosition();
		_currentTurnsToAttack = TurnsToAttack;
		_isAttackReady = false;
		IsAttacking = false;
	}

	private void Move(Direction direction)
	{
		GridPosition = _gameBoard.MoveEnemy(GridPosition, direction);

		UpdateWorldPosition();
	}

	private void UpdateWorldPosition()
	{
		OnMoving?.Invoke();

		var newPosition = _gameBoard.GetWorldPosition(GridPosition.x, GridPosition.y);

		if (transform.position.x - newPosition.x < 0)
		{
			transform.localScale = new Vector3(-1, 1, 1);
		}
		else
		{
			transform.localScale = new Vector3(1, 1, 1);
		}

		transform.DOMove(newPosition, 0.5f).OnComplete(() =>
		{
			if (transform.position.x < 0)
			{
				transform.localScale = new Vector3(-1, 1, 1);
			}
			else
			{
				transform.localScale = new Vector3(1, 1, 1);
			}

			IsMoving = false;
			OnIdle?.Invoke();
		});
	}

	// TODO: Provavelmente não precisa mais disso aqui
	private void OnDestroy()
	{
		//_gameBoard.ReleasePosition(GridPosition);
		//IsMoving = false;
		//OnEnemyDestroyed?.Invoke(this);
	}
}
