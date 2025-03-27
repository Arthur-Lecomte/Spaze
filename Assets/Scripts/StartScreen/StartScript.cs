using SmallHedge.SoundManager;
using UnityEngine;

public class StartScript : MonoBehaviour {
    private GameObject canvasParametre;
    private GameObject canvasStartMenu;
    private AudioSource musicAudioSource;

    /// <summary>
    /// Méthode appelée lors de l'initialisation de l'objet. Initialise les composants nécessaires.
    /// </summary>
    public void Awake() {
        canvasParametre = GameObject.Find("Canvas-Parametres");
        canvasStartMenu = GameObject.Find("Canvas-StartMenu");
        musicAudioSource = gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// Méthode appelée au démarrage. Joue la musique de l'écran de démarrage et masque le canvas des paramètres.
    /// </summary>
    public void Start() {
        SoundManager.PlaySoundWithFade(SoundType.STARTSCREEN, musicAudioSource, 5f);
        canvasParametre.SetActive(false);
    }

    /// <summary>
    /// Charge la scène spécifiée.
    /// </summary>
    /// <param name="sceneName">Le nom de la scène à charger.</param>
    public void LoadScene(string sceneName) {
        SoundManager.PlaySound(SoundType.CLICK);
        musicAudioSource.Stop();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Ouvre le menu des paramètres.
    /// </summary>
    public void OpenSettings() {
        SoundManager.PlaySound(SoundType.CLICK);
        canvasParametre.SetActive(true);
        canvasStartMenu.SetActive(false);
    }

    /// <summary>
    /// Ferme le menu des paramètres.
    /// </summary>
    public void CloseSettings() {
        SoundManager.PlaySound(SoundType.CLICK);
        canvasParametre.SetActive(false);
        canvasStartMenu.SetActive(true);
    }

    /// <summary>
    /// Quitte le jeu.
    /// </summary>
    public void QuitGame() {
        Application.Quit();
    }
}
