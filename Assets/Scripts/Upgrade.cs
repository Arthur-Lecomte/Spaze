using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour {
    public int id;
    private Image img;
    
    void Awake() {
        img = GetComponent<Image>();
    }

    public void NewValue(int nb) {
        img.color = nb < id ? Color.red : Color.green;
    }
}
