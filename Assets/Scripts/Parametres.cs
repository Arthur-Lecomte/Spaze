using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
namespace Spaze {
    public class Parametres : MonoBehaviour {
        [Header("Sliders")]
        private Slider soundEffectsSlider;
        private Slider musicSlider;

        private const string SoundEffectsVolumeParam = "SoundEffectsVolume";
        private const string MusicVolumeParam = "MusicVolume";

        private const string SoundEffectsPrefKey = "SoundEffectsVolume";
        private const string MusicPrefKey = "MusicVolume";

        private AudioMixer audioMixer;
        private GameObject startMenu;

        /// <summary>
        /// Initialise les param�tres au d�marrage, trouve les sliders et initialise leurs valeurs.
        /// </summary>
        private void Start() {
            startMenu = GameObject.Find("Canvas-StartMenu");

            // Trouver tous les sliders dans les enfants
            Slider[] sliders = GetComponentsInChildren<Slider>(true);

            foreach (Slider slider in sliders) {
                if (slider.name.Contains("SoundEffect")) {
                    soundEffectsSlider = slider;
                } else if (slider.name.Contains("Musique")) {
                    musicSlider = slider;
                }
            }

            audioMixer = SoundManager.instance.audioMixer;

            InitializeSliders();
        }

        /// <summary>
        /// Initialise les sliders avec les valeurs sauvegard�es et ajoute des listeners pour les changements de valeur.
        /// </summary>
        private void InitializeSliders() {
            float soundEffectsVolume = PlayerPrefs.GetFloat(SoundEffectsPrefKey, 1f);
            float musicVolume = PlayerPrefs.GetFloat(MusicPrefKey, 1f);

            soundEffectsSlider.value = soundEffectsVolume;
            musicSlider.value = musicVolume;

            SetSoundEffectsVolume(soundEffectsVolume);
            SetMusicVolume(musicVolume);

            soundEffectsSlider.onValueChanged.AddListener(SetSoundEffectsVolume);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        /// <summary>
        /// D�finit le volume des effets sonores et sauvegarde la valeur.
        /// </summary>
        /// <param name="value">La nouvelle valeur du volume des effets sonores.</param>
        public void SetSoundEffectsVolume(float value) {
            if (value <= 0.01f) {
                audioMixer.SetFloat(SoundEffectsVolumeParam, -80f);
            } else {
                float volumeInDb = Mathf.Log10(value) * 20;
                audioMixer.SetFloat(SoundEffectsVolumeParam, volumeInDb);
            }

            PlayerPrefs.SetFloat(SoundEffectsPrefKey, value); // Sauvegarder la valeur
        }

        /// <summary>
        /// D�finit le volume de la musique et sauvegarde la valeur.
        /// </summary>
        /// <param name="value">La nouvelle valeur du volume de la musique.</param>
        public void SetMusicVolume(float value) {
            if (value <= 0.01f) {
                audioMixer.SetFloat(MusicVolumeParam, -80f);
            } else {
                float volumeInDb = Mathf.Log10(value) * 20;
                audioMixer.SetFloat(MusicVolumeParam, volumeInDb);
            }

            PlayerPrefs.SetFloat(MusicPrefKey, value); // Sauvegarder la valeur
        }

        /// <summary>
        /// Ferme le menu des param�tres et r�active le menu de d�marrage.
        /// </summary>
        public void closeSettings() {
            gameObject.SetActive(false);
            Time.timeScale = 1;
            if (startMenu != null) {
                startMenu.SetActive(true);
            }
            
        }

        public void QuitGame() {
            Application.Quit();
        }
    }
}