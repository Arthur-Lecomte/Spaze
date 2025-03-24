using UnityEngine;

public class StartScript : MonoBehaviour {

    public void LoadScene(string sceneName) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void OpenSettings() {
        //GameObject.Find("Canvas-Parametres").GetComponent<Parametres>().CloseButton();
    }

    public void QuitGame() {
        Application.Quit();
    }
}
