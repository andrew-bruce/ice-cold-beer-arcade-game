using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class MenuItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool IsActive { get; private set; }

   // public Outline Outline { get; private set; }

    public Button Button { get; private set; }

    [SerializeField]
    private bool isButton;

    [SerializeField]
    private MenuSelectScript nextPanel;

    [SerializeField]
    private MenuSelectScript currentPanel;


    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }


    // Start is called before the first frame update
    void Start()
    {
        //Outline = GetComponent<Outline>();
        controls.Menu.MenuSelect.performed += ctx => MenuSelect();
        //controls.Menu.MenuBack.performed += ctx => MenuBack();
        if (isButton)
        {
            Button = GetComponentInParent<Button>();
        }
    }

    private void OnEnable()
    {
        controls.Menu.Enable();
    }

    private void OnDisable()
    {
        controls.Menu.Disable();
    }

    public void Activate()
    {
        try
        {
            GetComponent<Outline>().enabled = true;
            IsActive = true;
        } catch(Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public void Deactivate()
    {
        GetComponent<Outline>().enabled = false;
        IsActive = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Outline>().enabled = true;
        //if (masterOutline != null)
        //{
        //    masterOutline.enabled = false;
        //}
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Outline>().enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MenuSelect();
    }

    private void MenuSelect()
    {
        if (isButton && IsActive)
        {
            Button.onClick.Invoke();
            currentPanel.IsActive = false;

            if (nextPanel != null)
            {
                nextPanel.IsActive = true;
            }
        }
    }

    //private void MenuBack()
    //{
    //    if (IsActive)
    //    {
    //        currentPanel.IsActive = false;
    //        if (nextPanel != null)
    //        {
    //            nextPanel.IsActive = true;
    //        }
    //    }
    //}
}
