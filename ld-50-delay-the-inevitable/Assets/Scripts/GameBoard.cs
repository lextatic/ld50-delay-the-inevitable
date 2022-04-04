using UnityEngine;

public enum Direction
{
	In,
	Right,
	Left,
	Out
}

public class GameBoard : MonoBehaviour
{
	[SerializeField]
	private Vector2Int _boardSize = new Vector2Int(20, 4);

	[SerializeField]
	private float _innerCircleRadius = 1.5f;

	[SerializeField]
	private float _layerRadiusIncrement = 1;

	private bool[,] _ocupationMatrix;

	public Vector2Int BoardSize { get => _boardSize; }

	public float BoardOutsideSize
	{
		get
		{
			return _innerCircleRadius + (_boardSize.y * _layerRadiusIncrement);
		}
	}

	public bool HasFreeSpawnPoints
	{
		get
		{
			int count = 0;

			for (int i = 0; i < _boardSize.x; i++)
			{
				if (_ocupationMatrix[i, _boardSize.y - 1]) count++;
			}

			return count < _boardSize.x;
		}
	}

	public Vector2Int SpawnEnemy()
	{
		int randPositionX = 0;

		do
		{
			randPositionX = Random.Range(0, _boardSize.x);
		}
		while (!IsValidPosition(randPositionX, _boardSize.y - 1));

		_ocupationMatrix[randPositionX, _boardSize.y - 1] = true;

		return new Vector2Int(randPositionX, _boardSize.y - 1);
	}

	public Vector2Int MoveEnemy(Vector2Int enemyCurrentPosition, Direction direction)
	{
		_ocupationMatrix[enemyCurrentPosition.x, enemyCurrentPosition.y] = false;

		CalculateNewPositionWithDirection(ref enemyCurrentPosition, direction);

		_ocupationMatrix[enemyCurrentPosition.x, enemyCurrentPosition.y] = true;

		return enemyCurrentPosition;
	}

	public void ReleasePosition(Vector2Int gridPosition)
	{
		_ocupationMatrix[gridPosition.x, gridPosition.y] = false;
	}

	public Vector2 GetWorldPosition(int x, int y)
	{
		var sectionAngle = (360f / _boardSize.x);
		var rotatedVector = Rotate(Vector2.right, (sectionAngle * x) - (sectionAngle / 2)).normalized;

		return rotatedVector * (_innerCircleRadius + (_layerRadiusIncrement / 2) + (y * _layerRadiusIncrement));
	}

	public bool IsValidPosition(Vector2Int enemyCurrentPosition, Direction direction)
	{
		CalculateNewPositionWithDirection(ref enemyCurrentPosition, direction);

		return IsValidPosition(enemyCurrentPosition.x, enemyCurrentPosition.y);
	}

	public void OcupySpot(Vector2Int gridPosition)
	{
		_ocupationMatrix[gridPosition.x, gridPosition.y] = true;
	}

	private bool IsValidPosition(int x, int y)
	{
		if (y < 0 || y >= _boardSize.y) return false;

		return !_ocupationMatrix[x, y];
	}

	private void Awake()
	{
		_ocupationMatrix = new bool[_boardSize.x, _boardSize.y];
	}

	private void CalculateNewPositionWithDirection(ref Vector2Int enemyCurrentPosition, Direction direction)
	{
		switch (direction)
		{
			case Direction.In:
				enemyCurrentPosition.y--;
				break;

			case Direction.Right:
				enemyCurrentPosition.x++;

				if (enemyCurrentPosition.x >= _boardSize.x)
				{
					enemyCurrentPosition.x = 0;
				}

				break;

			case Direction.Left:
				enemyCurrentPosition.x--;

				if (enemyCurrentPosition.x < 0)
				{
					enemyCurrentPosition.x = _boardSize.x - 1;
				}

				break;

			case Direction.Out:
				enemyCurrentPosition.y++;
				break;
		}
	}

	public static Vector2 Rotate(Vector2 vector, float delta)
	{
		delta = Mathf.Deg2Rad * delta;

		return new Vector2(
			vector.x * Mathf.Cos(delta) - vector.y * Mathf.Sin(delta),
			vector.x * Mathf.Sin(delta) + vector.y * Mathf.Cos(delta)
		);
	}
}
