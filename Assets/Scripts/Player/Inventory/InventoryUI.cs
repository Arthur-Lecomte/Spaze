using UnityEngine;

public class InventoryUI : MonoBehaviour {
    public static InventoryUI Instance;

    [SerializeField] private GameObject slotPrefab;
    private RectTransform rectTransform;
    private int numberSlotsMemory;
    
    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        
        rectTransform = GetComponent<RectTransform>();
        numberSlotsMemory = 0;
    }
    
    public void ChangeNumberSlots(int numberSlots) {
        int columns = (numberSlots + 1) / 2;
        rectTransform.sizeDelta = new Vector2(15 + 105 * columns, 250);
        rectTransform.anchoredPosition = new Vector2(rectTransform.sizeDelta.x / 2 + 25, 150);
        
        for (int i = numberSlotsMemory + 1; i < numberSlots + 1; i++) {
            int column = (i + 1) / 2;
            GameObject slot = Instantiate(slotPrefab, rectTransform);
            RectTransform slotRectTransform = slot.GetComponent<RectTransform>();
            slotRectTransform.anchoredPosition = new Vector3(-45 + 105 * column, -65 + 105 * (i % 2), 0);
        }
        numberSlotsMemory = numberSlots;
    }
}