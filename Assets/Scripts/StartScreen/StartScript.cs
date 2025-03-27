using SmallHedge.SoundManager;
using UnityEngine;

public class StartScript : MonoBehaviour {
    private GameObject canvasParametre;
    private GameObject canvasStartMenu;

    

    
    private AudioSource musicAudioSource;

    public void Awake() {
        canvasParametre = GameObject.Find("Canvas-Parametres");
        canvasStartMenu = GameObject.Find("Canvas-StartMenu");
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        
    }

    public void Start(){
        SoundManager.PlaySoundWithFade(SoundType.STARTSCREEN, musicAudioSource, 5f);
        canvasParametre.SetActive(false);
    }
        
    

    public void LoadScene(string sceneName) {
        SoundManager.PlaySound(SoundType.CLICK);
        musicAudioSource.Stop();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void OpenSettings() {
        SoundManager.PlaySound(SoundType.CLICK);
        canvasParametre.SetActive(true);
        canvasStartMenu.SetActive(false);
        
    }

    public void CloseSettings() {
        SoundManager.PlaySound(SoundType.CLICK);
        canvasParametre.SetActive(false);
        canvasStartMenu.SetActive(true);
        
    }

    public void QuitGame() {
        Application.Quit();
    }
}
