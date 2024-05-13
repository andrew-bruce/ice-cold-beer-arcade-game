using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [SerializeField]
    private List<Button> Buttons;

    [SerializeField]
    private GameObject PausePanel;

    [SerializeField]
    private GameObject GameOverPanel;

    private AudioManager audioManager;
    private bool paused = false;
    private bool gameOver = false;
    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        audioManager = FindObjectOfType<AudioManager>();
        Buttons[0].onClick.AddListener(ContinueGame);
        Buttons[1].onClick.AddListener(ExitGame);
        Buttons[2].onClick.AddListener(RestartGame);
        Buttons[3].onClick.AddListener(ExitGame);
    }

    private void Start()
    {
        controls.Menu.MenuBack.performed += ctx => MenuBack();
        controls.Menu.Pause.performed += ctx => TogglePause();
    }

    private void OnEnable()
    {
        controls.Menu.Enable();
    }

    private void OnDisable()
    {
        controls.Menu.Disable();
    }

    private void MenuBack()
    {
        if (paused || gameOver)
        {
            ContinueGame();
        }
    }

    private void TogglePause()
    {
        paused = !paused;
        if (Time.timeScale == 1)
        {
            PausePanel.SetActive(true);
            PausePanel.GetComponent<MenuSelectScript>().IsActive = true;

            Time.timeScale = 0;

            audioManager.KillAllSoundEffects();
            audioManager.PlayMenuMove();
        }
        else
        {
            ContinueGame();
        }
    }

    public void GameOver()
    {
        gameOver = true;

        GameOverPanel.SetActive(true);
        GameOverPanel.GetComponent<MenuSelectScript>().IsActive = true;

        Time.timeScale = 0;

        audioManager.KillAllSoundEffects();
        audioManager.Play(Constants.Sounds.WrongBuzzer, Constants.VolumeLevels.WrongBuzzer);
    }

    private void RestartGame()
    {
        if (gameOver)
        {
            audioManager.PlayMenuSelect();
            SceneManager.LoadScene("GameScene");
            Time.timeScale = 1;
            gameOver = false;
        }
    }

    private void ContinueGame()
    {
        audioManager.PlayMenuSelect();
        PausePanel.SetActive(false);
        Time.timeScale = 1;
        paused = false;
    }

    private void ExitGame()
    {
        if (paused || gameOver)
        {
            SceneManager.LoadScene("MenuScene");
            Time.timeScale = 1;
            audioManager.KillAllSoundEffects();
            audioManager.PlayMenuSelect();
        }
    }
}
