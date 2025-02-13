using UnityEngine;

public class Asteroid : MonoBehaviour {
    public int ressource;
    private bool isCollecting;
    private float countdown;

    void Start() {
        ressource = Random.Range(5, 9);
    }
    
    void Update() {
        float distance = Vector3.Distance(PlayerMove.Instance.transform.position, transform.position);

        if (distance <= 8f) {
            if (!isCollecting) {
                isCollecting = true;
                countdown = 2f;
            }

            countdown -= Time.deltaTime * ModeBuild.Instance.pourcentageResource;
            if (countdown <= 0f) {
                ModeBuild.Instance.ChangeResource(GetRessources(ModeBuild.Instance.nbResource));
                countdown = 1f;
            }
        } else {
            isCollecting = false;
        }
    }

    public int GetRessources(int amount) {
        int available = Mathf.Min(ressource, amount);
        ressource -= available;
        CheckRessource();
        return available;
    }

    private void CheckRessource() {
        if (ressource <= 0) {
            Destroy(gameObject);
        }
    }
}
