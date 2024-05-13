using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingsScript : MonoBehaviour
{
    //public static OptionsScript instance;

    [SerializeField]
    private GameObject AudioOnIcon;

    [SerializeField]
    private GameObject AudioOffIcon;

    [SerializeField]
    private TextMeshProUGUI VolumeText;

    [SerializeField]
    private Slider Volume;

    [SerializeField]
    private string type;

    private float volume;
    private AudioManager audioManager;
    private PlayerControls controls;
    private float unmutedVolume = 0;

    private void OnEnable()
    {
        controls.Menu.Enable();
    }

    private void OnDisable()
    {
        controls.Menu.Disable();
    }

    void Awake()
    {
        controls = new PlayerControls();
        audioManager = FindObjectOfType<AudioManager>();
        Volume.onValueChanged.AddListener(delegate { SetVolume(); });
        volume = audioManager.GetVolume(type) * 100;
    }


    private void Update()
    {
        if (Volume.value != volume)
        {
            Volume.value = volume;
            SetVolume();
        }
    }

    public void VolumeUp()
    {
        Volume.value++;
        SetVolume();
    }

    public void VolumeDown()
    {
        Volume.value--;
        SetVolume();
    }

    public void Mute()
    {
        unmutedVolume = Volume.value;
        Volume.value = 0;
        SetVolume();
    }

    public void Unmute()
    {
        Volume.value = unmutedVolume;
        SetVolume();
    }

    private void SetVolume()
    {
        volume = Volume.value;
        VolumeText.text = $"{volume}%";

        if (volume == 0)
        {
            AudioOnIcon.SetActive(false);
            AudioOffIcon.SetActive(true);
        }
        else
        {
            AudioOnIcon.SetActive(true);
            AudioOffIcon.SetActive(false);
        }

        audioManager.SetVolume(volume/100, type);
    }
}
