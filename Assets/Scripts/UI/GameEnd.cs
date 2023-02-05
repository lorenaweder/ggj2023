using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private int _nextScene;
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _other;
    [SerializeField] private Button _next;
    [SerializeField] private TextMeshProUGUI _time;
    [SerializeField] private string _textFormat;

    private void Awake()
    {
        var t = transform as RectTransform;

        t.anchoredPosition = Vector3.zero;

        _next.interactable = false;
        _image.DOFade(0f, 0);

        _other.SetActive(false);
    }

    void Start()
    {
        MessageDispatcher.OnGameOver += ShowResult;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        MessageDispatcher.OnGameOver -= ShowResult;
        _next.onClick.RemoveAllListeners();
    }

    public void LoadRetry()
    {
        _next.onClick.RemoveAllListeners();
        SceneManager.LoadScene(_nextScene);
    }

    private void ShowResult()
    {
        var time = FindObjectOfType<GameTimer>().TimerTime;
        _time.SetText(string.Format(_textFormat, (TimeSpan.FromSeconds(time)).ToString("mm\\:ss")));
        _next.onClick.AddListener(LoadRetry);
        gameObject.SetActive(true);
        DOTween.Sequence()
            .Append(_image.DOFade(1f, 0.25f))
            .AppendCallback(EnableButtons)
            .Append(_image.DOFade(0f, 0.2f));
    }

    private void EnableButtons()
    {
        EventSystem.current.SetSelectedGameObject(_next.gameObject);

        _next.interactable = true;
        _other.SetActive(true);
        _image.raycastTarget = false;
    }
}
