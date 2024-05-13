using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionsHeadingScript : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI heading;

    private void Start()
    {
        int index = heading.text.IndexOf("Panel");
        string newHeading = (index < 0)
            ? heading.text
            : heading.text.Remove(index, "Panel".Length);

        heading.text = newHeading;
    }
}
