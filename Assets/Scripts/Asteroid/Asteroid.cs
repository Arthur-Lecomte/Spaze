using UnityEngine;
//using Ressources.RessourcesUI;

public class Asteroid : MonoBehaviour
{
    private bool isCollecting;
    private float countdown;

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(Vaisseau.Instance.transform.position, transform.position);

        if (distance <= 8f) {
            if (!isCollecting) {
                isCollecting = true;
                countdown = 2f;
            }

            countdown -= Time.deltaTime * 0.4f;
            if (countdown <= 0f) {

                foreach(TypeRessource type in System.Enum.GetValues(typeof(TypeRessource))) {
                    RessourcesUI.Instance.AddRessourceByType(type, RandomRessourcesValue(type));
                }
                countdown = 1f;
            }
        } else {
            isCollecting = false;
        }
    }

    private int RandomRessourcesValue(TypeRessource ressourceType)
    {
        int ramdomValue = Random.Range(0, 100 + 1);
        int returnRessourceValue;
        switch (ressourceType)
        {
            case TypeRessource.Cuivre :
                returnRessourceValue = ramdomValue <= 50 ? Random.Range(1, 30 + 1) : 0;
                break;
            case TypeRessource.Argent :
                returnRessourceValue = ramdomValue <= 30 ? Random.Range(1, 15 + 1) : 0;
                break;
            case TypeRessource.Or :
                returnRessourceValue = ramdomValue <= 15 ? Random.Range(1, 5 + 1) : 0;
                break;
            case TypeRessource.PoussiereRadioactive :
                returnRessourceValue = ramdomValue <= 5 ? Random.Range(1, 2 + 1) : 0;
                break;
            default:
                returnRessourceValue = 0;
                break;
        }
        return returnRessourceValue;
    }
}
