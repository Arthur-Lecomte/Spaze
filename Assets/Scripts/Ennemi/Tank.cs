using UnityEngine;
public class Tank : Ennemi
{
    void Start() {
        health = 1000;
        moveSpeed = 0.3f; // Très lent
        shootInterval = 2.5f;
        shootRange = 5f; // Tire de près
        timeBeforeBeingCible = 3f;
    }
}