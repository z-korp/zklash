using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    // function qui s'execure avant toute les autres fonctions meme avant le Start
    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de AudioManager dans la scène");
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    // Un audio clip est un élément audio mp3 wav flac etc ...
    public AudioClip[] playlist;
    public AudioSource source;

    public AudioMixerGroup audioMixerGroup;
    private int musicIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        // On charge le premier élément de la playlist
        source.clip = playlist[0];
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        playNextSong();
    }

    void playNextSong()
    {
        // On check si le son est en cours de lecture
        if (!source.isPlaying)
        {
            // Sinon on passe à la musique suivante de la playlist si on a plus d'éléments on bouche car % playlist.Length est égale à 0
            musicIndex = (musicIndex + 1) % playlist.Length;
            source.clip = playlist[musicIndex];
            source.Play();
        }
    }

    public AudioSource PlayClipAt(AudioClip clip, Vector3 position)
    {
        GameObject temporaryGameObjectAudio = new GameObject("Audio Object");
        temporaryGameObjectAudio.transform.position = position;
        // On ajoute le composant AudioSource à l'objet audio et en plus on crée un AudioSource pour l'objet audio qu'on va pouvoir acceder pour modifier ses paramêtres
        AudioSource audioSource = temporaryGameObjectAudio.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.Play();
        // On détruit l'objet audio quand le son est fini, en effet on peut facilement avoir la durée d'un son avec clip.length
        Destroy(temporaryGameObjectAudio, clip.length);
        return audioSource;
    }
}
