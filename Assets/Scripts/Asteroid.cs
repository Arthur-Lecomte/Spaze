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
                countdown = 3f;
            }

            countdown -= Time.deltaTime;
            if (countdown <= 0f) {
                ModeBuild.Instance.ChangeResource(GetRessources(1));
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
