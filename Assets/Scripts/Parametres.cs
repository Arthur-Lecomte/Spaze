using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Parametres : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Sliders")]
    [SerializeField] private Slider soundEffectsSlider;
    [SerializeField] private Slider musicSlider;

    private const string SoundEffectsVolumeParam = "SoundEffectsVolume";
    private const string MusicVolumeParam = "MusicVolume";

    private void Start()
    {
        // Initialiser les sliders avec les valeurs actuelles de l'Audio Mixer
        float soundEffectsVolume;
        float musicVolume;

        audioMixer.GetFloat(SoundEffectsVolumeParam, out soundEffectsVolume);
        audioMixer.GetFloat(MusicVolumeParam, out musicVolume);

        soundEffectsSlider.value = Mathf.Pow(10, soundEffectsVolume / 20); // Convertir dB en pourcentage
        musicSlider.value = Mathf.Pow(10, musicVolume / 20);
    }

    public void SetSoundEffectsVolume(float value)
    {
        if (value <= 0.01f) // Si le slider est à 0 ou proche de 0
        {
            audioMixer.SetFloat(SoundEffectsVolumeParam, -80f); // Mettre le volume a 0
        }
        else
        {
            float volumeInDb = Mathf.Log10(value) * 20; // Convertir le pourcentage en dB
            audioMixer.SetFloat(SoundEffectsVolumeParam, volumeInDb);
        }
    }

    public void SetMusicVolume(float value)
    {
        if (value <= 0.01f) 
        {
            audioMixer.SetFloat(MusicVolumeParam, -80f); 
        }
        else
        {
            float volumeInDb = Mathf.Log10(value) * 20; 
            audioMixer.SetFloat(MusicVolumeParam, volumeInDb);
        }
    }
}