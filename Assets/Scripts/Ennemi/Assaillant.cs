using UnityEngine;
public class Assaillant : Ennemi
{ 
    void Start() {
        // Initialiser les valeurs spécifiques pour l'Assaillant
        health = 200;
        moveSpeed = 10f;
        shootInterval = 1f;
        shootRange = 10f; 
        timeBeforeBeingCible = 2f;
    }
}