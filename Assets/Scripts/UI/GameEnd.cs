using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button next;

    private void Awake()
    {
        next.interactable = false;
        image.DOFade(0f, 0);
    }

    void Start()
    {
        MessageDispatcher.OnGameOver += ShowResult;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        MessageDispatcher.OnGameOver -= ShowResult;
    }

    private void ShowResult()
    {
        gameObject.SetActive(true);
        DOTween.Sequence()
            .Append(image.DOFade(1f, 0.25f))
            .AppendCallback(EnableButtons);
    }

    private void EnableButtons()
    {
        next.interactable = true;
    }
}
