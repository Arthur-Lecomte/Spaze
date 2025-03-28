using UnityEngine;
using UnityEngine.SceneManagement;

namespace Spaze
{
    /// <summary>
    /// Classe pour gérer l'écran de fin de jeu.
    /// </summary>
    /// 
    public class GameOver : MonoBehaviour
    {
        /// <summary>
        /// Méthode pour quitter
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
        }

        /// <summary>
        /// Méthode pour redémarrer le jeu.
        /// </summary>
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
