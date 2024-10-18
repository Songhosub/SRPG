using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager
{
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    AudioSource[] audioSources = new AudioSource[Enum.GetNames(typeof(Define.Sound)).Length];
    /*
    public float masterVolume { get; protected set; }
    public float bgmVolume { get; protected set; }
    public float effectVolume { get; protected set; }
    */
    public float[] Volumes = new float[3];

public void Init()
    {
        GameObject root = GameObject.Find("@Soound_Root");
        
        if (root == null)
        {
            root = new GameObject { name = "@Soound_Root" };
            GameObject.DontDestroyOnLoad(root);
            
            for (int i = 0; i < audioSources.Length; i++)
            {
                GameObject go = new GameObject { name = Enum.GetName(typeof(Define.Sound), i) };
                audioSources[i] = go.GetorAddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }
            
            audioSources[(int)Define.Sound.BGM].loop = true;
        }

        for(int i = 0; i < Volumes.Length; i++)
        {
            Volumes[i] = 1.0f;
        }
    }

    public void VolumeSetting(Define.Sound sound, Scrollbar scrollbar, Text text)
    {
        switch (sound)
        {
            case Define.Sound.BGM:
                Volumes[1] = scrollbar.value;
                audioSources[(int)Define.Sound.BGM].volume = Volumes[1] * Volumes[0];
                break;
            case Define.Sound.Effect:
                Volumes[2] = scrollbar.value;
                audioSources[(int)Define.Sound.Effect].volume = Volumes[2] * Volumes[0];
                break;
            default:
                Volumes[0] = scrollbar.value;
                audioSources[(int)Define.Sound.BGM].volume = Volumes[1] * Volumes[0];
                audioSources[(int)Define.Sound.Effect].volume = Volumes[2] * Volumes[0];
                break;
        }
        text.text = $"{(Mathf.RoundToInt(scrollbar.value * 10) * 10)}%";
    }

    public void Play(AudioClip clip, Define.Sound type)
    {
        if (clip == null)
        {
            return;
        }
        
        if (type == Define.Sound.BGM)
        {
            AudioSource audioSource = audioSources[(int)Define.Sound.BGM];
            
            if(audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            
            audioSource.volume = Volumes[1] * Volumes[0];
            audioSource.clip = clip;
            audioSource.Play();
        }
        
        else
        {
            AudioSource audioSource = audioSources[(int)Define.Sound.Effect];
            audioSource.volume = Volumes[2] * Volumes[0];
            audioSource.PlayOneShot(clip);
        }
    }

    public void Play(string path, Define.Sound type, string weapon = "")
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type, weapon);
        Play(audioClip, type);
    }

    public AudioClip GetOrAddAudioClip(string path, Define.Sound type, string weapon)
    {       
        if(type == Define.Sound.BGM)
        {
            if (!audioClips.TryGetValue($"BGM_{path}", out AudioClip clip))
            {
                clip = Managers.resource.Load<AudioClip>($"Sounds/BGM_{path}");
                audioClips.Add($"BGM_{path}", clip);
                return clip;
            }

            return clip;
        }

        if(type == Define.Sound.Effect)
        {
            if (!audioClips.TryGetValue($"Effect_{weapon}{path}", out AudioClip clip))
            {
                clip = Managers.resource.Load<AudioClip>($"Sounds/Effect_{weapon}{path}");
                if (clip != null)
                {
                    audioClips.Add($"Effect_{weapon}{path}", clip);
                    return clip;
                }

                else if (!audioClips.TryGetValue($"Effect_{path}", out clip))
                {
                    clip = Managers.resource.Load<AudioClip>($"Sounds/Effect_{path}");

                    if (clip != null)
                    {
                        audioClips.Add($"Effect_{path}", clip);
                        return clip;
                    }
                }
            }

            return clip;
        }

        return null;
    }

    public void Clear()
    {
        foreach(AudioSource source in audioSources)
        {
            source.Stop();
            source.clip = null;
        }
        
        audioClips.Clear();
    }
}