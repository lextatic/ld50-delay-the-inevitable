using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeInImage : MonoBehaviour
{
	public Image Image;

	public float AlphaValue;

	public float Time;

	void Start()
	{
		Image.DOFade(AlphaValue, Time);
	}
}
