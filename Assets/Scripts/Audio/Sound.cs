using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    //just for getting things started will expend upon it later
    public string name;
    public AudioClip audioClip;
    [Range(0f, 1f)]
    public float volume;
    [Range(0, 3)]
    public float pitch;
    [HideInInspector]
    public AudioSource audioSource;
    public bool loop;
}
