using UnityEngine;
using System.Collections;

public class SpawnEnnemi : MonoBehaviour {
    public GameObject ennemiPrefab; // Préfabriqué de l'ennemi
    public Transform player; // Référence au joueur
    public Camera mainCamera; // Référence à la caméra principale

    void Start() {
        StartCoroutine(SpawnEnnemiRoutine());
    }

    IEnumerator SpawnEnnemiRoutine() {
        while (true) {
            float waitTime = Random.Range(2f, 5f);
            yield return new WaitForSeconds(waitTime);
            SpawnEnnemiOutsideView();
        }
    }

    void SpawnEnnemiOutsideView() {
        Vector3 spawnPosition = GetRandomPositionOutsideView();
        GameObject ennemi = Instantiate(ennemiPrefab, spawnPosition, Quaternion.identity);
        Ennemi ennemiScript = ennemi.GetComponent<Ennemi>();
        if(ennemiScript) {
            ennemiScript.player = player;
        }
    }

    Vector3 GetRandomPositionOutsideView() {
        int distance = 30;
        
        float x = Random.Range(-distance, distance + 1);
        float z = Random.Range(-distance, distance + 1);

        // Choose a random direction and move 105 units in that direction
        int direction = Random.Range(0, 4);
        switch (direction) {
            case 0:
                z = distance;
                break;
            case 1:
                z = -distance;
                break;
            case 2:
                x = distance;
                break;
            case 3:
                x = -distance;
                break;
        }
        
        return new Vector3(mainCamera.transform.position.x + x, 0, mainCamera.transform.position.z + z);
    }
}
