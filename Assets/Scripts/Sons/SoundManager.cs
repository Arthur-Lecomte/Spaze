using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace SmallHedge.SoundManager
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundsSO SO;
        [SerializeField] public AudioMixer audioMixer; // Référence à l'Audio Mixer
        [SerializeField] private AudioMixerGroup soundEffectsGroup; // Groupe pour les effets sonores
        [SerializeField] private AudioMixerGroup musicGroup;


        public static SoundManager instance = null;
        private AudioSource audioSource;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject); // Rendre le SoundManager persistant
                if (audioSource == null)
                {
                    audioSource = gameObject.AddComponent<AudioSource>();
                }
            }
            else
            {
                Destroy(gameObject); // Détruire les instances supplémentaires
            }
        }

        public static void PlaySound(SoundType sound, AudioSource source = null, float volume = 1)
        {
            SoundList soundList = instance.SO.sounds[(int)sound];
            AudioClip[] clips = soundList.sounds;
            AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

            if (source)
            {
                source.outputAudioMixerGroup = soundList.mixer;
                source.clip = randomClip;
                source.volume = volume * soundList.volume;
                source.Play();
            }
            else
            {
                instance.audioSource.outputAudioMixerGroup = soundList.mixer;
                instance.audioSource.PlayOneShot(randomClip, volume * soundList.volume);
            }
        }

        public static void PlaySoundWithFade(SoundType sound, AudioSource source, float fadeDuration, float volume = 1)
        {

            SoundList soundList = instance.SO.sounds[(int)sound];
            AudioClip[] clips = soundList.sounds;
            AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

            // Configurer l'AudioSource
            source.outputAudioMixerGroup = soundList.mixer;
            source.clip = randomClip;
            source.volume = 0f; // Commencer avec un volume à 0
            source.Play();

            // Lancer le fondu
            instance.StartCoroutine(FadeAudio(source, volume * soundList.volume, fadeDuration));
        }

        private static IEnumerator FadeAudio(AudioSource audioSource, float targetVolume, float duration)
        {
            if (audioSource == null)
            {
                yield break;
            }

            float startVolume = audioSource.volume;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                if (audioSource == null)
                {
                    yield break;
                }

                elapsedTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
                yield return null;
            }
            if (audioSource != null)
            {
                audioSource.volume = targetVolume;

                if (targetVolume == 0)
                {
                    audioSource.Stop();
                }
            }
        }

        public static void StopSoundWithFade(AudioSource source, float fadeDuration)
        {
            if (source == null)
            {
                Debug.LogError("AudioSource est null. Impossible d'arrêter le son avec un fondu.");
                return;
            }

            // Lancer le fondu pour réduire le volume à 0
            instance.StartCoroutine(FadeAudio(source, 0f, fadeDuration));
        }

    }

    [Serializable]
    public struct SoundList
    {
        [HideInInspector] public string name;
        [Range(0, 1)] public float volume;
        public AudioMixerGroup mixer;
        public AudioClip[] sounds;
    }
}