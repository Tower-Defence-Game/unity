using Interfaces.ObjectAbilities;
using Interfaces.ObjectProperties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(IHavePreStart))]
public class StartManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    public Button StartButton => startButton;

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
        GetComponent<IHavePreStart>().IsLevelStarted = true;

        Time.timeScale = 1f;

        // Hides the button
        startButton.gameObject.SetActive(false);
    }
}
