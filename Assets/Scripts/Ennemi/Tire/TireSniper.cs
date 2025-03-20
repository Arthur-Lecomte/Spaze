using UnityEngine;

public class TireSniper : Tire
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    speed = 35f; // Vitesse de déplacement
    damage = 300; // Dégâts infligés
    Destroy(gameObject, 3f);
  
    }

}
