using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Vector2Int GridPosition;

	private GameBoard _gameBoard;

	public void AIMove()
	{
		var randomChance = Random.value;

		// 60% forward, 30% side, 10% back
		if (randomChance <= 0.6f)
		{
			MoveSafe(Direction.In);
		}
		else if (randomChance <= 0.6f + 0.15f)
		{
			MoveSafe(Direction.Right);
		}
		else if (randomChance <= 0.6f + 0.15f + 0.15f)
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
	}

	private void Awake()
	{
		_gameBoard = GameObject.FindWithTag("Managers").GetComponent<GameBoard>();
	}

	private void Start()
	{
		UpdateWorldPosition();
	}

	private void Move(Direction direction)
	{
		GridPosition = _gameBoard.MoveEnemy(GridPosition, direction);

		UpdateWorldPosition();
	}

	private void UpdateWorldPosition()
	{
		transform.position = _gameBoard.GetWorldPosition(GridPosition.x, GridPosition.y);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			Move(Direction.Out);
		}

		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			Move(Direction.Right);
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			Move(Direction.Left);
		}

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			Move(Direction.In);
		}

		if (Input.GetKeyDown(KeyCode.O))
		{
			AIMove();
		}
	}
}
