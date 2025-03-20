using UnityEngine;
public class Sniper : Ennemi
{
    void Start() {
        health = 150;
        moveSpeed = 0.5f;
        shootInterval = 3f; // Tire lentement
        shootRange = 25f; // Tire de loin
        timeBeforeBeingCible = 1.5f;
    }
}