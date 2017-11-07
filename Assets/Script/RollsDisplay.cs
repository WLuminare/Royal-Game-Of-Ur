using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RollsDisplay : MonoBehaviour
{
    public TextMeshProUGUI TextHeader;
    public TextMeshProUGUI Text;

    public void Show()
    {
        TextHeader.enabled = true;
        Text.enabled = true;
        Text.text = GameController.obj.RolledValue.ToString();
    }

    public void Hide()
    {
        TextHeader.enabled = false;
        Text.enabled = false;
    }
}
