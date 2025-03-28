using UnityEngine;
namespace Spaze {
    public class DeathManager : MonoBehaviour {
        public static DeathManager Instance;

        [SerializeField] public GameObject[] objectsToDisable;
        [SerializeField] public GameObject[] objectsToEnable;

        /// <summary>
        /// Initialise l'instance du DeathManager. Si une instance existe déjà, détruit le GameObject actuel.
        /// </summary>
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Gère les actions à effectuer lors de la fin du jeu.
        /// Active et désactive les objets spécifiés et joue le son de fin de jeu.
        /// </summary>
        public void GameOver() {
            gameObject.SetActive(true);
            Vaisseau.Instance.gameObject.SetActive(false);
            SoundManager.PlaySound(SoundType.GAMEOVER);

            foreach (var obj in objectsToDisable) {
                if (obj != null)
                    obj.SetActive(false);
            }

            foreach (var obj in objectsToEnable) {
                if (obj != null)
                    obj.SetActive(true);
            }
        }
    }
}