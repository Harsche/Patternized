using System.Collections;
using System.Collections.Generic;
using PrototypePattern.Input;
using PrototypePattern.Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private Image _endGamePanelImage;
    [SerializeField] private GameObject _aim;
    [SerializeField] private PlayerController _player;
    [SerializeField] private TMP_Text _endGameText;

    private void Awake()
    {
        SetGameState(isEndGame: false);
    }

    public void ShowVictory()
    {
        ShowEndGame(true);
    }

    public void ShowDefeat()
    {
        ShowEndGame(false);
    }

    private void ShowEndGame(bool victory)
    {
        SetGameState(isEndGame: true);
        if (_endGameText != null)
            _endGameText.text = victory ? "Victory!" : "Defeat!";

        _endGamePanelImage.color = victory ? Color.black : new Color32(0xAC, 0x39, 0x39, 0xFF);
    }

    private void SetGameState(bool isEndGame)
    {
        DisablePlayerComponents(!isEndGame);
        _aim.SetActive(!isEndGame);
        Cursor.visible = isEndGame;
        Time.timeScale = isEndGame ? 0f : 1f;
        _endGamePanel.SetActive(isEndGame);
    }

    public void DisablePlayerComponents(bool isEndGame)
    {
        InputHandler inputHandler = _player.GetComponent<InputHandler>();
        PlayerMovement playerMovement = _player.GetComponent<PlayerMovement>();
        inputHandler.enabled = isEndGame;
        playerMovement.enabled = isEndGame;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
