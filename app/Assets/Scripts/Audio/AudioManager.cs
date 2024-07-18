using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public enum SoundEffect
{
    Swap,
    Hire,
    Explosion,
    Stun
}

public class AudioManager : Singleton<AudioManager>
{

    // Playlist for village and battle themes
    public AudioClip[] villagePlaylist;
    public AudioClip[] battlePlaylist;
    public AudioSource musicSource; // Assigned to handle background music

    // Separate AudioSource for sound effects
    public AudioSource effectSource; // Assigned to handle sound effects

    public AudioMixerGroup audioMixerGroup; // Optional: Remove if not using

    private int musicIndex = 0;

    // Enum to define the current theme
    public enum Theme { Village, Battle }
    public Theme currentTheme = Theme.Village;

    // Dictionary to map sound effects to audio clips
    private Dictionary<SoundEffect, AudioClip> soundEffects;

    public List<SoundEffectClip> soundEffectClips;

    [System.Serializable]
    public struct SoundEffectClip
    {
        public SoundEffect soundEffect;
        public AudioClip audioClip;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        // Initialize the sound effects dictionary
        soundEffects = new Dictionary<SoundEffect, AudioClip>();
        foreach (var soundEffectClip in soundEffectClips)
        {
            soundEffects[soundEffectClip.soundEffect] = soundEffectClip.audioClip;
        }

        // Start with the village theme
        PlayNextSong();
    }

    private void Update()
    {
        if (!musicSource.isPlaying)
        {
            PlayNextSong();
        }
    }

    private void PlayNextSong()
    {
        AudioClip[] playlist = GetCurrentPlaylist();
        if (playlist.Length == 0)
            return;

        musicIndex = (musicIndex + 1) % playlist.Length;
        musicSource.clip = playlist[musicIndex];
        musicSource.Play();
    }

    private AudioClip[] GetCurrentPlaylist()
    {
        switch (currentTheme)
        {
            case Theme.Battle:
                return battlePlaylist;
            case Theme.Village:
            default:
                return villagePlaylist;
        }
    }

    public AudioSource PlayClipAt(AudioClip clip, Vector3 position)
    {
        GameObject temporaryGameObjectAudio = new GameObject("Audio Object");
        temporaryGameObjectAudio.transform.position = position;
        AudioSource audioSource = temporaryGameObjectAudio.AddComponent<AudioSource>();
        audioSource.clip = clip;

        // Optional: Set the audioMixerGroup if you are using it
        if (audioMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = audioMixerGroup;
        }

        audioSource.Play();
        Destroy(temporaryGameObjectAudio, clip.length);
        return audioSource;
    }

    public void PlaySoundEffect(SoundEffect soundEffect, Vector3 position)
    {
        if (soundEffects.TryGetValue(soundEffect, out AudioClip clip))
        {
            PlayClipAt(clip, position);
        }
        else
        {
            Debug.LogWarning("SoundEffect not found: " + soundEffect);
        }
    }

    public void SwitchTheme(Theme newTheme)
    {
        StartCoroutine(FadeOutAndSwitchTheme(newTheme));
    }

    private IEnumerator FadeOutAndSwitchTheme(Theme newTheme)
    {
        float startVolume = musicSource.volume;

        // Fade out
        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / 1.0f; // Adjust the time for fade-out duration
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume;

        // Switch theme
        currentTheme = newTheme;
        musicIndex = -1; // Reset music index to start new theme from the first track
        PlayNextSong();

        // Fade in
        musicSource.volume = 0;
        while (musicSource.volume < startVolume)
        {
            musicSource.volume += startVolume * Time.deltaTime / 1.0f; // Adjust the time for fade-in duration
            yield return null;
        }

        musicSource.volume = startVolume;
    }
}