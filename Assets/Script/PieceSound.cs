using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceSound : MonoBehaviour {

    public AudioClip endTile;
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
        source.clip = endTile;
        source.playOnAwake = false;
    }
    public void PlaySounds()
    {
        source.PlayOneShot(endTile);
    }
}
