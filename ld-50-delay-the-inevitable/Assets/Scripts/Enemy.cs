using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Vector2Int GridPosition;

	private GameBoard _gameBoard;

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
	}
}
