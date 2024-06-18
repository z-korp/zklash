using System.Collections.Generic;
//using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    // Audio Mixer est présent dans Windows > Audio > Audio Mixer
    // Comme d'hab il fait bien penser à l'ajouter en drag and drop dans le script.
    public AudioMixer audioMixer;
    public Dropdown resolutionDropDown;
    public Slider musicSlider;
    public Slider effectsSlider;

    Resolution[] resolutions;

    public void Start()
    {
        // On reccupère le volume pour set les sliders
        audioMixer.GetFloat("Music", out float musicVolumeValueForSlider);
        musicSlider.value = musicVolumeValueForSlider;

        audioMixer.GetFloat("Effects", out float effectsVolumeValueForSlider);
        effectsSlider.value = effectsVolumeValueForSlider;
        // On réccupère les résolutions disponibles dans la variable resolutions
        resolutions = Screen.resolutions;
        // Si jamais les resolutions sont dupliqué voici comment faire on utlise Linq qui sert normalement pour les db sql et on vérifier que les éléments soient distinctent
        //resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height}).Distinct().ToArray();

        resolutionDropDown.options.Clear();
        List<string> options = new List<string>();


        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            // On réccupère la résolution en string
            string option = resolutions[i].width + " x " + resolutions[i].height;
            // On peut facilement ajouter comme c'est une list est pas un array
            options.Add(option);

            // On réccupère la résolution actuelle
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        // On ajoute les options dans le dropdown
        resolutionDropDown.AddOptions(options);
        // On sélectionne la résolution actuelle et on positionne le dropdown à la bonne option
        resolutionDropDown.value = currentResolutionIndex;
        // Cette méthode permet de rafraichir l'élément actuellement sélectionné.
        resolutionDropDown.RefreshShownValue();

        Screen.fullScreen = true;
    }



    // Quand on vient ajouter un evenement, on va donc ajouter le parent qui contient le script et directement l'UI de unity va detecter que dans ma fonction SetVolume j'ai en paramêtre un float, l'ui va donc passer automatiquement le float du slider à la fonction SetVolume.
    public void SetMusicVolume(float volume)
    {
        // On réccupère la valeur du float qui vient de l'ui on la passe à l'audio mixer que l'on vient d'exposer.
        audioMixer.SetFloat("Music", volume);
    }

    public void SetEffectsVolume(float volume)
    {
        // On réccupère la valeur du float qui vient de l'ui on la passe à l'audio mixer que l'on vient d'exposer.
        audioMixer.SetFloat("Effects", volume);
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    // Methode on passe l'index de la résolution dans la liste de résolution
    public void SetResolution(int resolutionIndex)
    {
        // On réccupère la résolution en fonction de l'index
        Resolution resolution = resolutions[resolutionIndex];
        // On passe la résolution à la méthode SetResolution de la classe Screen
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

}
