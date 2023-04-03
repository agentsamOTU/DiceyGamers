using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    public List<AudioClip> audioClip;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        instance = this;
    }

    public void PlayClip(int clip)
    {
        source.clip = audioClip[clip];
        source.Play();
    }
}
