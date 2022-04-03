using DG.Tweening;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;

	public float AlphaValue;

	public float Time;

	void Start()
	{
		spriteRenderer.DOFade(AlphaValue, Time);
	}
}
