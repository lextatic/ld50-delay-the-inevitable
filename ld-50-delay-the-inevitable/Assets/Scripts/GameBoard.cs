using UnityEngine;

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

	public Vector2 SpawnEnemy()
	{
		int randPositionX = 0;

		do
		{
			randPositionX = Random.Range(0, _boardSize.x);
		}
		while (!IsPositionFree(randPositionX, _boardSize.y - 1));

		_ocupationMatrix[randPositionX, _boardSize.y - 1] = true;

		return GetWorldPosition(randPositionX, _boardSize.y - 1);
	}

	private void Start()
	{
		_ocupationMatrix = new bool[_boardSize.x, _boardSize.y];
	}

	private bool IsPositionFree(int x, int y)
	{
		return !_ocupationMatrix[x, y];
	}

	private Vector2 GetWorldPosition(int x, int y)
	{
		var sectionAngle = (360f / _boardSize.x);
		var rotatedVector = Rotate(Vector2.right, (sectionAngle * x) - (sectionAngle / 2)).normalized;

		return rotatedVector * (_innerCircleRadius + (_layerRadiusIncrement / 2) + (y * _layerRadiusIncrement));
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		UnityEditor.Handles.color = Color.green;

		UnityEditor.Handles.DrawWireDisc(Vector3.zero, Vector3.forward, _innerCircleRadius);
		for (int layer = 0; layer < _boardSize.y; layer++)
		{
			UnityEditor.Handles.DrawWireDisc(Vector3.zero, Vector3.forward, _innerCircleRadius + _layerRadiusIncrement + (layer * _layerRadiusIncrement));
		}

		for (int i = 0; i < _boardSize.x; i++)
		{
			var rotatedVector = Rotate(Vector2.right, (360f / _boardSize.x) * i).normalized;
			Gizmos.DrawLine(rotatedVector * _innerCircleRadius, rotatedVector * (_innerCircleRadius + _boardSize.y * _layerRadiusIncrement));

			for (int layer = 0; layer < _boardSize.y; layer++)
			{
				Gizmos.DrawSphere(GetWorldPosition(i, layer), 0.1f);
			}
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
