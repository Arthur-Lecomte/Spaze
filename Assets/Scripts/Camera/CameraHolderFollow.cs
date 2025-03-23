using UnityEngine;

public class CameraHolderFollow : MonoBehaviour
{
    private void LateUpdate()
    {
        //Suit le joueur
        transform.position = Vaisseau.Instance.transform.position;
    }
}
