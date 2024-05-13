using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderLinkScript : MonoBehaviour
{
    [SerializeField]
    private Slider master;

    [SerializeField]
    private Slider music;

    [SerializeField]
    private Slider sfx;

    [SerializeField]
    private List<MenuItem> menuItems;

    private int selectedMenuItemIndex;

    private AudioManager audioManager;

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnEnable()
    {
        controls.Menu.Enable();
    }

    private void OnDisable()
    {
        controls.Menu.Disable();
    }

    private void Start()
    {
        //controls.Menu.VolumeMenuUp.performed += ctx => VolumeSelectionUp();
        //controls.Menu.VolumeMenuDown.performed += ctx => VolumeSelectionDown();
    }

    private void Update()
    {
        if (FindActive() != null)
        {
            if (controls.Menu.VolumeUp.IsPressed())
            {
                FindActive().GetComponentInParent<VolumeSettingsScript>().VolumeUp();
            }

            if (controls.Menu.VolumeDown.IsPressed())
            {
                FindActive().GetComponentInParent<VolumeSettingsScript>().VolumeDown();
            }
        

        if (controls.Menu.VolumeUp.WasReleasedThisFrame() || controls.Menu.VolumeDown.WasReleasedThisFrame())
        {
            switch (selectedMenuItemIndex)
            {
                case 0:
                    MasterVolumeChanged();
                    break;
                case 1:
                    MusicVolumeChanged();
                    break;
                case 2:
                    SfxVolumeChanged();
                    break;
            }
        }

        }
    }

    private MenuItem FindActive()
    {
        if (menuItems.Any() && menuItems.First() != null)
        {
            var active = menuItems.FirstOrDefault(m => m.IsActive);
            if (active != null)
            {
                selectedMenuItemIndex = menuItems.IndexOf(active);
                return active;
            }
        }

        return null;
    }

    //private void VolumeSelectionUp()
    //{
    //    if (selectedSliderIndex > 0)
    //    {
    //        audioManager.PlayMenuMove();
    //        selectedSliderIndex--;
    //        sliders[selectedSliderIndex].GetComponentInChildren<Outline>().enabled = true;
    //        sliders[selectedSliderIndex + 1].GetComponentInChildren<Outline>().enabled = false;
    //    }
    //}

    //private void VolumeSelectionDown()
    //{
    //    if (selectedSliderIndex < 2)
    //    {
    //        audioManager.PlayMenuMove();
    //        selectedSliderIndex++;
    //        sliders[selectedSliderIndex].GetComponentInChildren<Outline>().enabled = true;
    //        sliders[selectedSliderIndex - 1].GetComponentInChildren<Outline>().enabled = false;
    //    }
    //}

    public void MasterVolumeChanged()
    {
        if (music.value > master.value)
        {
            music.value = master.value;
        }

        if (sfx.value > master.value)
        {
            sfx.value = master.value;
        }

        audioManager.PlayExample(master.value/100);
    }

    public void MusicVolumeChanged()
    {
        if (music.value > master.value)
        {
            master.value = music.value;
        }

        audioManager.PlayExample(music.value / 100);
    }

    public void SfxVolumeChanged()
    {
        if (sfx.value > master.value)
        {
            master.value = sfx.value;
        }

        audioManager.PlayExample(sfx.value / 100);
    }
}
