using SmallHedge.SoundManager;
using UnityEngine;

public class DeathManager : MonoBehaviour {
    public static DeathManager Instance;
    
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
