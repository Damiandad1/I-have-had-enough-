using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _startGame;
    [SerializeField] private Button _options;
    [SerializeField] private Button _exit;

    private int _firstGame = 1;
    
    private void Awake()
    {
        _startGame.onClick.AddListener(StartGame);
        _options.onClick.AddListener(Options);
        _exit.onClick.AddListener(Exit);
    }

    private void StartGame()
    {
        SceneManager.LoadScene(_firstGame);
    }
    private void Options()
    {
        // function with settings
    }
    private void Exit()
    {
        Application.Quit();
    }
}
