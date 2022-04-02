using UnityEngine;

public class MarkView : MonoBehaviour
{
	public GameObject Mark;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Mark.SetActive(true);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Mark.SetActive(false);
	}
}
