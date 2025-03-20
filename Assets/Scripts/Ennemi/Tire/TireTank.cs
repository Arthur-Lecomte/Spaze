using UnityEngine;

public class TireTank :Tire
{
    void Start() 
    {
    speed = 5f; // Vitesse de déplacement
    damage = 25; // Dégâts infligés
    Destroy(gameObject, 3f);

    }
}
