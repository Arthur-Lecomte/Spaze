using UnityEngine;
public class Invocateur : Ennemi
{
    public GameObject minionPrefab; // Préfabriqué du sbire

    void Start() {
        health = 300;
        moveSpeed = 0.7f;
        shootInterval = 4f;
        shootRange = 8f;
        timeBeforeBeingCible = 2f;
    }

    void ShootAtPlayer() {
        if (minionPrefab) {
            GameObject minion = Instantiate(minionPrefab, transform.position + transform.right * 1.5f, Quaternion.identity);
            minion.GetComponent<Ennemi>().player = player; // Le sbire attaque aussi le joueur
        }
    }
}