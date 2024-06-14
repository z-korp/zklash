using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one AudioManager in the scene!");
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // an AudioClip is an audio element mp3 wav flac etc ...
    public AudioClip[] playlist;
    public AudioSource source;

    public AudioMixerGroup audioMixerGroup;
    private int musicIndex = 0;

    void Start()
    {
        // load the first song of the playlist
        source.clip = playlist[0];
        source.Play();
    }

    void Update()
    {
        playNextSong();
    }

    void playNextSong()
    {
        if (!source.isPlaying)
        {
            // we use % to loop the playlist
            musicIndex = (musicIndex + 1) % playlist.Length;
            source.clip = playlist[musicIndex];
            source.Play();
        }
    }

    public AudioSource PlayClipAt(AudioClip clip, Vector3 position)
    {
        GameObject temporaryGameObjectAudio = new GameObject("Audio Object");
        temporaryGameObjectAudio.transform.position = position;
        // we add the AudioSource component to the audio object and in addition we create an AudioSource for the audio object that we will be able to access to modify its parameters
        AudioSource audioSource = temporaryGameObjectAudio.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.Play();
        // we destroy the audio object when the sound is finished, indeed we can easily have the duration of a sound with clip.length
        Destroy(temporaryGameObjectAudio, clip.length);
        return audioSource;
    }

}
