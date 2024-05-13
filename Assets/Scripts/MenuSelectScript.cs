using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuSelectScript : MonoBehaviour
{
    public bool IsActive = false;

    [SerializeField]
    private List<GameObject> uiElements;

    private List<MenuItem> menuItems = new();

    private int selectedIndex = 0;
    private PlayerControls controls;

    [SerializeField]
    private MenuSelectScript nextPanel;

    [SerializeField]
    private MenuSelectScript currentPanel;

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

    // Start is called before the first frame update
    void Start()
    {
        controls.Menu.MenuUp.performed += ctx => MenuUp();
        controls.Menu.MenuDown.performed += ctx => MenuDown();
        controls.Menu.MenuBack.performed += ctx => MenuBack();

        foreach (var element in uiElements)
        {
            var mi = element.GetComponent<MenuItem>();
            menuItems.Add(mi);
        }

        if (menuItems.Count > 0)
        {
            menuItems.First().Activate();
        }
    }

    private void MenuBack()
    {
        if (IsActive)
        {
            currentPanel.IsActive = false;
            if (nextPanel != null)
            {
                nextPanel.IsActive = true;
            }
        }
    }


    private void MenuUp()
    {
        if (selectedIndex > 0 && IsActive)
        {
            selectedIndex--;
            menuItems[selectedIndex].Activate();
            menuItems[selectedIndex + 1].Deactivate();
        }
    }

    private void MenuDown()
    {
        if (selectedIndex < uiElements.Count - 1 && IsActive)
        {
            selectedIndex++;
            menuItems[selectedIndex].Activate();
            menuItems[selectedIndex - 1].Deactivate();
        }
    }
}
