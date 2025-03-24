using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    
    [SerializeField] public GameObject[] objectsToDisable;
    [SerializeField] public GameObject[] objectsToEnable;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void GameOver() {
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
