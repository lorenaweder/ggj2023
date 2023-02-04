using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDispatcher : MonoBehaviour
{
    public static event Action OnGameOver;
    public static void NotifyGameOver() => OnGameOver?.Invoke();

    public static event Action OnGameStarted;
    public static void NotifyGameStarted() => OnGameStarted?.Invoke();
}
