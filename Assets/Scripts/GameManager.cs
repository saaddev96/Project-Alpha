using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GameManager : Singleton<GameManager>
{

    [SerializeField] private PlayerInput _playerInput;

    [SerializeField]private InputReader _inputReader;
    [SerializeField] private GameObject settingPanel;
    public bool IsGamePaused { get; private set;}

    private void OnEnable()
    {
        _inputReader.PauseEvent += GamePause;
    }
    private void OnDisable()
    {
        _inputReader.PauseEvent -= GamePause;
    }
    private void Start()
    {
        IsGamePaused = false;
    }
    public void GamePause()
    {
        IsGamePaused = !IsGamePaused;
        if (IsGamePaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _playerInput.SwitchCurrentActionMap("UI");
            settingPanel.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _playerInput.SwitchCurrentActionMap("GamePlay");
            settingPanel.SetActive(false);
        }
    }
}
