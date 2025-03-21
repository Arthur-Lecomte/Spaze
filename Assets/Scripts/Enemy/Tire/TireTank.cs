using UnityEngine;

public class TireTank :Tire
{
    void Start() 
    {
    speed = 5f; // Vitesse de déplacement
    Destroy(gameObject, 3f);

    }
}
