using UnityEngine;

public class CameraHolderFollow : MonoBehaviour
{
    [SerializeField] private Transform vaisseau;

    private void LateUpdate()
    {
        //Suit le joueur
        transform.position = vaisseau.position;
    }
}
