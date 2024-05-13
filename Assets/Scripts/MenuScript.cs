using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Assets.Scripts;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    private List<Button> Buttons;

    [SerializeField]
    private Button CloseOptionsButton;

    public GameObject HowToPlayPanel;
    public GameObject OptionsPanel;

    private AudioManager audioManager;
    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Menu.Enable();
    }

    private void OnDisable()
    {
        controls.Menu.Disable();
    }

    void Start()
    { 
        Buttons[0].onClick.AddListener(PlayGame);
        Buttons[1].onClick.AddListener(HowToPlay);
        Buttons[2].onClick.AddListener(Options);
        Buttons[4].onClick.AddListener(CloseOptions);
        Buttons[3].onClick.AddListener(CloseHowToPlay);
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void PlayGame()
    {
        audioManager.PlayMenuSelect();
        SceneManager.LoadScene("GameScene");
        audioManager.Play(Constants.Sounds.Music, Constants.VolumeLevels.Music);
    }

    private void HowToPlay()
    {
        audioManager.PlayMenuSelect();
        HowToPlayPanel.SetActive(true);
    }

    private void CloseHowToPlay()
    {
        audioManager.PlayMenuSelect();
        HowToPlayPanel.SetActive(false);
    }

    private void Options()
    {
        audioManager.PlayMenuSelect();
        OptionsPanel.SetActive(true);
    }

    private void CloseOptions()
    {
        audioManager.PlayMenuSelect();
        OptionsPanel.SetActive(false);
    }
}
