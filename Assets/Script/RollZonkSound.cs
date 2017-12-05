using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollZonkSound : MonoBehaviour
{

    public AudioClip zonk;
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
        source.clip = zonk;
        source.playOnAwake = false;
    }
    public void PlaySoundZonk()
    {
        source.PlayOneShot(zonk);
    }
}
