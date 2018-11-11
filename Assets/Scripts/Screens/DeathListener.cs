using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathListener : MonoBehaviour
{
    public Button StartButton;
    public Button QuitButton;

    public Text ScoreText;

    void Start()
    {
        StartButton.onClick.AddListener(StartGame);
        QuitButton.onClick.AddListener(Quit);

        ScoreText.text = $"Score: {GlobalState.Score}";
    }

    void StartGame()
    {
#if UNITY_EDITOR
        Debug.Log("Starting a new game");
#else
        SceneManager.LoadScene("Game");
#endif
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        SceneManager.LoadScene("MainMenu");
#endif
    }
}
