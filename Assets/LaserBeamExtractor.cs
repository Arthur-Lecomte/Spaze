using System.Security.Principal;
using UnityEngine;

public class LaserBeamExtractor : MonoBehaviour
{

    public GameObject laserPrefab;
    public GameObject firePoint;
    private GameObject laser;

    void Start()
    {
        laser = Instantiate(laserPrefab, firePoint.transform);
        DisableLaser();
    }

    
    

    public void EnableLaser(){
        laser.SetActive(true);
    }

    public void DisableLaser(){
        laser.SetActive(false);
    }

    public void UpdateLaser(){
        if(firePoint != null){
            laser.transform.position = firePoint.transform.position;
        }
    }
}


