using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private Sound[] _sounds;

    private void Awake()
    {
        if (FindObjectOfType<AudioManager>() != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound sound in _sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
        }
    }

    public void PlaySound(string name)
    {
        Sound foundSound = Array.Find(_sounds, sound => sound.name == name);
        if (foundSound == null)
        {
            Debug.Log($"No sound with " + name + " found");
            return;
        }
        if (!foundSound.audioSource.isPlaying)
        {
            foundSound.audioSource.Play();
        }
    }
    public void PlaySoundAtSpot(string name, Vector3 position, float volume)
    {
        Sound foundSound = Array.Find(_sounds, sound => sound.name == name);
        if (foundSound == null)
        {
            Debug.Log($"No sound with " + name + " found");
            return;
        }
        if (!foundSound.audioSource.isPlaying)
        {
            AudioSource.PlayClipAtPoint(foundSound.audioClip, position, 1f);
        }
    }
    public void PlaySoundOneTime(string name)
    {
        Sound foundSound = Array.Find(_sounds, sound => sound.name == name);
        if (foundSound == null)
        {
            Debug.Log($"No sound with " + name + " found");
            return;
        }
        foundSound.audioSource.PlayOneShot(foundSound.audioClip);
    }

    public void StopSound(string name)
    {
        Sound foundSound = Array.Find(_sounds, sound => sound.name == name);
        if (foundSound == null)
        {
            Debug.Log($"No sound with " + name + " found");
            return;
        }
        if (foundSound.audioSource.isPlaying)
        {
            foundSound.audioSource.Stop();
        }
    }

    public void ChangePitch(string name, float pitch)
    {
        Sound foundSound = Array.Find(_sounds, sound => sound.name == name);
        if (foundSound == null)
        {
            Debug.Log($"No sound with " + name + " found");
            return;
        }
        foundSound.audioSource.pitch = pitch;
    }
    public float GetPitch(string name)
    {
        Sound foundSound = Array.Find(_sounds, sound => sound.name == name);
        if (foundSound == null)
        {
            Debug.Log($"No sound with " + name + " found");
            return 0;
        }
        return foundSound.audioSource.pitch;
    }
    public void SetVolume(string name, float volume)
    {
        Sound foundSound = Array.Find(_sounds, sound => sound.name == name);
        if (foundSound == null)
        {
            Debug.Log($"No sound with " + name + " found");
            return;
        }
        foundSound.audioSource.volume = volume;
    }
    public float GetVolume(string name)
    {
        Sound foundSound = Array.Find(_sounds, sound => sound.name == name);
        if (foundSound == null)
        {
            Debug.Log($"No sound with " + name + " found");
            return 0;
        }
        return foundSound.audioSource.volume;
    }
    public Sound GetSound(string name)
    {
        return Array.Find(_sounds, sound => sound.name == name);
    }
}
