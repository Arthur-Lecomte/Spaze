
using UnityEngine;
namespace Spaze {
    public class StartScript : MonoBehaviour {
        private GameObject canvasParametre;
        private GameObject canvasStartMenu;
        private AudioSource musicAudioSource;

        /// <summary>
        /// M�thode appel�e lors de l'initialisation de l'objet. Initialise les composants n�cessaires.
        /// </summary>
        public void Awake() {
            canvasParametre = GameObject.Find("Canvas-Parametres");
            canvasStartMenu = GameObject.Find("Canvas-StartMenu");
            musicAudioSource = gameObject.AddComponent<AudioSource>();
        }

        /// <summary>
        /// M�thode appel�e au d�marrage. Joue la musique de l'�cran de d�marrage et masque le canvas des param�tres.
        /// </summary>
        public void Start() {
            SoundManager.PlaySoundWithFade(SoundType.STARTSCREEN, musicAudioSource, 5f);
            canvasParametre.SetActive(false);
        }

        /// <summary>
        /// Charge la sc�ne sp�cifi�e.
        /// </summary>
        /// <param name="sceneName">Le nom de la sc�ne � charger.</param>
        public void LoadScene(string sceneName) {
            SoundManager.PlaySound(SoundType.CLICK);
            musicAudioSource.Stop();
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Ouvre le menu des param�tres.
        /// </summary>
        public void OpenSettings() {
            SoundManager.PlaySound(SoundType.CLICK);
            canvasParametre.SetActive(true);
        }

        /// <summary>
        /// Ferme le menu des param�tres.
        /// </summary>
        public void CloseSettings() {
            SoundManager.PlaySound(SoundType.CLICK);
            canvasParametre.SetActive(false);
        }

        /// <summary>
        /// Quitte le jeu.
        /// </summary>
        public void QuitGame() {
            Application.Quit();
        }
    }
}