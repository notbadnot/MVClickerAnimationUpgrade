using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIWindowAnimator : MonoBehaviour
{
	private Sequence sequence;
	[SerializeField]private CanvasGroup canvasGroup;

	void Start()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();		
	}

    private void OnEnable()
    {
		canvasGroup = gameObject.GetComponent<CanvasGroup>();
		sequence = DOTween.Sequence();
		sequence.Insert(0f, canvasGroup.DOFade(0f, 0.0001f));
		sequence.Insert(0.0001f, canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.OutBounce));

		sequence.SetTarget(this);
		sequence.Play();
	}


    private void OnDisable()
	{
		if (DOTween.IsTweening(this))
		{
			this.DOKill();
		}
	}

	private void Update()
	{
		if (Input.anyKey)
		{
			if (DOTween.IsTweening(this))
			{
				Debug.Log("imkilled");
				this.DOComplete(true);
			}
		}
	}
}
