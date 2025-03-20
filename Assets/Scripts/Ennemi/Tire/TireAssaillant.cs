using UnityEngine;

public class TireAssaillant : Tire
{
    void Start()
    {
    speed = 10f; // Vitesse de déplacement
    damage = 100; // Dégâts infligés
    Destroy(gameObject, 3f);

    }

}
