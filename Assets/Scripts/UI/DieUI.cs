using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieUI : MonoBehaviour
{
    public Image backgroundImage;
    public Image iconImage;

    public void SelectIcon()
    {
        backgroundImage.color = new Color(1, 1, 1, 1);
        iconImage.color = new Color(1, 1, 1, 1);
    }

    public void DeselectIcon()
    {
        backgroundImage.color = new Color(1, 1, 1, 0.5f);
        iconImage.color = new Color(1, 1, 1, 0.5f);
    }
}
