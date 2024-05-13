using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using Unity.VisualScripting;
using Assets.Scripts.Extensions;
using Assets.Scripts;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private float masterVolume;
    private float musicVolume;
    private float sfxVolume;

    public void PlayExample(float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == Constants.Sounds.MenuSelect);

        s.source.volume = volume;

        s.source.Play();
    }

    public void PlayMenuMove()
    {
        Play(Constants.Sounds.MenuMove, Constants.VolumeLevels.MenuMove);
    }

    public void PlayMenuSelect()
    {
        Play(Constants.Sounds.MenuSelect, Constants.VolumeLevels.MenuSelect);
    }

    public float GetVolume(string type)
    {
        switch(type)
        {
            case "master":
                return masterVolume;
            case "music":
                return musicVolume;
            case "sfx":
                return sfxVolume;
            default:
                return masterVolume;
        }
    }

    public void SetVolume(float volume, string type)
    {
        switch (type)
        {
            case "master":
                masterVolume = volume;
                break;
            case "music":
                musicVolume = volume;
                break;
            case "sfx":
                sfxVolume = volume;
                break;
        }
    }

    private float RemapVolume(float localVolume, float volumeSetting)
    {
        return localVolume.Remap(0, 1, 0, volumeSetting);
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            masterVolume = 1;
            musicVolume = 1;
            sfxVolume = 1;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = sound.playOnAwake;
        }
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound with name " + s.name + " does not exist");
            return false;
        }

        return s.source.isPlaying;
    }

    public void Play(string name, float volume)
    {
        if (masterVolume <= 0)
        {
            return;
        }

        if (musicVolume <= 0 && name == Constants.Sounds.Music)
        {
            return;
        }

        if (sfxVolume <= 0 && name != Constants.Sounds.Music)
        {
            return;
        }

        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.Log("Sound with name " + s.name + " does not exist");
            return;
        }

        if (!s.repeatableSound && s.source.isPlaying)
        {
            return;
        }

        var remappedVolume = volume;
        if (masterVolume != 1)
        {
            remappedVolume = RemapVolume(volume, masterVolume);
        }
        
        if (sfxVolume != 1 && name != Constants.Sounds.Music && sfxVolume < masterVolume)
        {
            remappedVolume = RemapVolume(volume, sfxVolume);
        }

        if (musicVolume != 1 && name == Constants.Sounds.Music && musicVolume < masterVolume)
        {
            remappedVolume = RemapVolume(volume, masterVolume);
        }

        s.source.volume = remappedVolume;

        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.Log("Sound with name " + s.name + " does not exist");
            return;
        }

        s.source.Stop();
    }

    public void SetVolume(string name, float volume)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }

        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.Log("Sound with name " + s.name + " does not exist");
            return;
        }

        float remappedVolume = volume;

        if (sfxVolume != 1 && name != Constants.Sounds.Music && sfxVolume < masterVolume)
        {
            remappedVolume = RemapVolume(volume, sfxVolume);
        }

        s.source.volume = remappedVolume;
    }

    public void SetPitch(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.Log("Sound with name " + s.name + " does not exist");
            return;
        }

        s.source.pitch = pitch;
    }

    public void FadeAudioOut(string name, float duration)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }

        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.Log("Sound with name " + s.name + " does not exist");
            return;
        }

        StartCoroutine(FadeAudio(s.source, duration, 0));
    }

    public void FadeAudioIn(string name, float duration, float targetVolume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.Log("Sound with name " + s.name + " does not exist");
            return;
        }

        StartCoroutine(FadeAudio(s.source, duration, targetVolume));
    }

    public IEnumerator FadeAudio(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }

        if (targetVolume == 0)
        {
            audioSource.Stop();
        }

        yield break;
    }

    public void KillAllSoundEffects()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.Stop();
        }
    }
}