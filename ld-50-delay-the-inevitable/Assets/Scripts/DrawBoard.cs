using UnityEngine;

public class DrawBoard : MonoBehaviour
{
	public int Subsections = 5;

	public float Lengh = 7;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		UnityEditor.Handles.color = Color.green;

		UnityEditor.Handles.DrawWireDisc(Vector3.zero, Vector3.forward, 1.5f);
		for (int layer = 0; layer < 4; layer++)
		{
			UnityEditor.Handles.DrawWireDisc(Vector3.zero, Vector3.forward, 2.5f + layer);
		}

		for (int i = 0; i < Subsections; i++)
		{
			var rotatedVector = Rotate(Vector2.right, (360f / Subsections) * i).normalized;

			Gizmos.DrawLine(rotatedVector * 1.5f, rotatedVector * Lengh);

			rotatedVector = Rotate(rotatedVector, -(360f / Subsections) / 2);

			for (int layer = 0; layer < 4; layer++)
			{
				Gizmos.DrawSphere(rotatedVector * (2 + layer), 0.1f);
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
