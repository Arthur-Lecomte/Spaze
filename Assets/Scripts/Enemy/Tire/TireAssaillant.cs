using UnityEngine;

public class TireAssaillant : Tire
{
    void Start()
    {
    speed = 10f; // Vitesse de déplacement
    Destroy(gameObject, 3f); // Détruire le projectile après 3 secondes

    }

}
