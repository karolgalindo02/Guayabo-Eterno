using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManagment : MonoBehaviour
{
   public static AudioManagment Instance;
   public Sound[] musicSounds, sfxSounds;
   [Header ("Audio Source")]
   public AudioSource musicSource, sfxSource;

    [Header ("Audio Clip")]
   public AudioClip Theme;
   public AudioClip Death;
   public AudioClip Step;
   public AudioClip Win;
   public AudioClip Jump;

    private void Awake() {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        musicSource.clip = Theme;
        musicSource.loop = true;
        musicSource.Play();
    }

    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound not found: " + name);
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
         sfxSource.PlayOneShot(clip);
    }
}