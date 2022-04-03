using DG.Tweening;
using TMPro;
using UnityEngine;

public class FadeInFadeOutText : MonoBehaviour
{
	public TextMeshProUGUI Text;

	public float Time;

	void Start()
	{
		Text.DOFade(0, Time).SetLoops(-1, LoopType.Yoyo);
	}
}
