using System.Collections;
using System.Collections.Generic;
using PrototypePattern.Input;
using PrototypePattern.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _aim;
    [SerializeField] private PlayerController _player;

    private void Awake()
    {
        SetGameState(isGameOver: false);
    }

    public void ShowGameOver()
    {
        SetGameState(isGameOver: true);
    }

    private void SetGameState(bool isGameOver)
    {
        DisablePlayerComponents(!isGameOver);

        _aim.SetActive(!isGameOver);
        Cursor.visible = isGameOver;
        Time.timeScale = isGameOver ? 0f : 1f;
        _gameOverPanel.SetActive(isGameOver);

    }
    public void DisablePlayerComponents(bool isGameOver)
    {
        InputHandler inputHandler = _player.GetComponent<InputHandler>();
        PlayerMovement playerMovement = _player.GetComponent<PlayerMovement>();

        inputHandler.enabled = isGameOver;
        playerMovement.enabled = isGameOver;
    }
    public void RestartGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
