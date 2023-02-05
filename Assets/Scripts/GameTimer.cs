using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private bool _timerOn;
    private float _time;

    [SerializeField] private TMPro.TextMeshProUGUI _display;

    void Start()
    {
        MessageDispatcher.OnGameStarted += StartTimer;
    }

    private void OnDestroy()
    {
        MessageDispatcher.OnGameStarted -= StartTimer;
        StopTimer();
    }

    private void StartTimer()
    {
        _timerOn = true;
    }

    private void UpdateText()
    {
        var t = TimeSpan.FromSeconds(_time);
        _display.SetText(t.ToString("mm\\:ss"));
    }

    private void StopTimer()
    {
        _timerOn = false;
    }

    void Update()
    {
        if (!_timerOn) return;
        _time += Time.deltaTime;

        if (Time.frameCount % 4 == 0) UpdateText();
    }
}
