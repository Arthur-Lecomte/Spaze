using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] public GameObject[] objectsToDisable;
    [SerializeField] public GameObject[] objectsToEnable;



    public void GameOver()
    {
        
        foreach (var obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        foreach (var obj in objectsToEnable)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }
}
