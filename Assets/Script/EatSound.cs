using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatSound: MonoBehaviour
{

    public AudioClip eat;
    public AudioSource source
    {
        get
        {
            return GetComponent<AudioSource>();
        }
    }
    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        source.clip = eat;
        source.playOnAwake = false;
    }
    public void PlaySoundEat()
    {
        source.PlayOneShot(eat);
    }
}
