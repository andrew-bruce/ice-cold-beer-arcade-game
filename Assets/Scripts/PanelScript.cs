using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelScript : MonoBehaviour
{
    private PlayerControls controls;
    private AudioManager audioManager;

    private void Awake()
    {
        controls = new PlayerControls();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Start()
    {
        controls.Menu.MenuBack.performed += ctx => Invoke(nameof(MenuBack), 0.05f);
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
        if (gameObject.activeSelf)
        {
            audioManager.PlayMenuSelect();
            gameObject.SetActive(false);
        }
    }
}
