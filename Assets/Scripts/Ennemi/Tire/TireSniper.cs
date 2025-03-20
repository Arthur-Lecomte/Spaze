using UnityEngine;

public class TireSniper : Tire
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    speed = 35f; // Vitesse de déplacement
    Destroy(gameObject, 3f);
  
    }

}
