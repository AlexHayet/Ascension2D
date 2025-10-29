using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [SerializeField] private SoundFX[] soundFXes;
    private Dictionary<string, List<AudioClip>> audioDict;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary() // Creates audio dictionary with audio clips
    {
        audioDict = new Dictionary<string, List<AudioClip>>();
        foreach (SoundFX SoundFX in soundFXes)
        {
            audioDict[SoundFX.name] = SoundFX.audioClips;
        }
    }

    public AudioClip MultiClip(string audioName) // Will choose from multiple different audio clips for the same action
    {
        if (audioDict.ContainsKey(audioName))
        {
            List<AudioClip> audioClips = audioDict[audioName];
            if (audioClips.Count > 0)
            {
                return audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
            }
        }
        return null;
        }
    }

    [System.Serializable]
    public struct SoundFX // For sound effect names and allotment of clips
    {
        public string name;
        public List<AudioClip> audioClips;
    }
