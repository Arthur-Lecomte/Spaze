using UnityEngine;
public class Assaillant : Ennemi
{ 
    void Start() {
        // Initialiser les valeurs spécifiques pour l'Assaillant
        damage = 200;
        health = 200;
        moveSpeed = 1f;
        shootInterval = 1f;
        shootRange = 10f; 
        timeBeforeBeingCible = 2f;
    }
}