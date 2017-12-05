using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RollSound : MonoBehaviour {
    public AudioClip sounds;
    private Button button
    {
        get
        {
            return GetComponent<Button>();
        }
    }
    private AudioSource source
    {
        get
        {
            return GetComponent<AudioSource>();
        }
    }
	// Use this for initialization
	void Start () {
        gameObject.AddComponent<AudioSource>();
        source.clip = sounds;
        source.playOnAwake = false;
        button.onClick.AddListener(() => PlaySound());
	}
	void PlaySound ()
    {
        source.PlayOneShot(sounds);
    }
}
