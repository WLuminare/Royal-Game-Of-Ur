using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationText : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public float Timer;
    public const float MESSAGE_TIMER = 2f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            Text.enabled = false;
        }
    }

    public void ShowMessage(GameController.PlayerNumber recipientPlayer, string message)
    {
        Text.enabled = true;
        Timer = MESSAGE_TIMER;
        Text.text = message;
    }
}
