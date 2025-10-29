using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundFXManager : MonoBehaviour
{
    private static SoundFXManager Instance;

    private static AudioSource audioSource;
    private static SoundEffects sfxLib;
    [SerializeField] private Slider soundBar;

    private void Awake() // Grabs sound effects from the library
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            sfxLib = GetComponent<SoundEffects>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName) // Play a sound one time when the action related to the sound occurs
    {
        AudioClip audioClip = sfxLib.MultiClip(soundName);
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
    // Start is called before the first frame update
    void Start() // On start have the volume set to what the soundBar is set to
    {
        soundBar.onValueChanged.AddListener(delegate { ChangingVol(); });
    }

    public static void SetVolume(float volume) // Sets the volume for all audio
    {
        audioSource.volume = volume;
    }

    public void ChangingVol() // Allows for changing of the volume on the sound bar / volume slider
    {
        SetVolume(soundBar.value);
    }
}
