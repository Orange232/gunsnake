﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private Sound[] sounds;
    private static Sound[] _sounds;
    [SerializeField]
    private Sound[] music;
    private static Sound[] _music;

    [SerializeField]
    private GameObject audioListenerObj;
    private static AudioLowPassFilter menuLowPass;

    public static AudioManager instance;

    private static string currentMusic;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        _sounds = sounds;
        _music = music;

        foreach (Sound s in _sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        foreach (Sound s in _music)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        menuLowPass = audioListenerObj.GetComponent<AudioLowPassFilter>();
    }

    private void Start()
    {

    }

    public static void Play(string name)
    {
        if (_sounds == null)
            return;
        Sound s = Array.Find(_sounds, sound => sound.name == name);
        
        if (s == null)
        {
            Debug.LogError("Sound: " + name + " not found!");
            return;
        }

        if (s.doRandomPitch)
            s.source.pitch = s.pitch * UnityEngine.Random.Range(.95f, 1.05f);

        s.source.Play();
    }

    public static void PlayMusic(string name)
    {
        if (_music == null)
            return;

        if (name == currentMusic)
            return;

        StopMusic();
        Sound s = Array.Find(_music, music => music.name == name);
        
        if (s == null)
        {
            Debug.LogError("Music: " + name + " not found!");
            return;
        }

        currentMusic = name;
        s.source.Play();
    }

    private static void StopMusic()
    {
        if (_music == null)
            return;

        foreach (Sound s in _music)
        {
            if (s == null || s.source == null)
                continue;
            s.source.Stop();
        }
    }

    public static void SetMusicVolume(float value)
    {
        value = Mathf.Clamp(value, 0, 1);

        if (_music == null)
            return;
        foreach (Sound s in _music)
        {
            if (s == null || s.source == null)
                continue;
            s.source.volume = s.volume * value;
        }
    }

    public static void SetLowPassEnabled(bool value)
    {
        menuLowPass.enabled = value;
    }

    public static void SetSfxVolume(float value)
    {
        value = Mathf.Clamp(value, 0, 1);

        if (_sounds == null)
            return;
        foreach (Sound s in _sounds)
        {
            if (s == null || s.source == null)
                continue;
            s.source.volume = s.volume * value;
        }
    }
}
