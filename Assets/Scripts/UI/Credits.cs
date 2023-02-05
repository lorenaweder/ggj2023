using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] private int _nextScene;
    [SerializeField] private Image _image;
    [SerializeField] private Button _back;
    [SerializeField] private GameObject _other;
    [SerializeField] private GameObject _creditsOnIntro;

    bool hid;

    private void Awake()
    {
        var t = transform as RectTransform;

        t.anchoredPosition = Vector3.zero;

        _back.interactable = false;

        _image.DOFade(0f, 0f);

        _other.SetActive(false);
        gameObject.SetActive(false);

        hid = true;
    }

    private void OnEnable()
    {
        if (!hid) return;

        DOTween.Sequence()
            .Append(_image.DOFade(1f, 0.25f))
            .AppendCallback(ShowContent)
            .Append(_image.DOFade(0f, 0.25f))
            .OnComplete(EnableButtons);
    }

    private void OnDestroy()
    {
        _back.onClick.RemoveAllListeners();
    }

    private void ShowContent()
    {
        _other.SetActive(true);
    }

    private void EnableButtons()
    {
        _back.interactable = true;
        _image.raycastTarget = false;

        _back.onClick.AddListener(Return);
        EventSystem.current.SetSelectedGameObject(_back.gameObject);
    }

    private void Return()
    {
        _back.interactable = false;
        _image.raycastTarget = true;
        _image.DOFade(1f, 0.25f).OnComplete(Hide);
    }

    private void Hide()
    {
        EventSystem.current.SetSelectedGameObject(_creditsOnIntro.gameObject);
        gameObject.SetActive(false);
    }
}
