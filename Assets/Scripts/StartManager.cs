using System;
using Interfaces.ObjectAbilities;
using Interfaces.ObjectProperties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(IHavePreStart))]
public class StartManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private List<GameObject> objectsToHideAfterStart;
    [SerializeField] private UnityEvent onStart;
    private static readonly UnityEvent OnStartInternal = new();
    public Button StartButton => startButton;

    public static void AddOnStart(UnityAction action)
    {
        OnStartInternal.AddListener(action);
    }

    void Start()
    {
        Time.timeScale = 0f;
    }

    void OnEnable()
    {
        startButton.onClick.AddListener(StartGame);
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(StartGame);
    }

    void StartGame()
    {
        Time.timeScale = 1f;

        onStart?.Invoke();
        OnStartInternal?.Invoke();
        startButton.gameObject.SetActive(false);
        foreach (var obj in objectsToHideAfterStart)
        {
            obj.SetActive(false);
        }
    }
}
