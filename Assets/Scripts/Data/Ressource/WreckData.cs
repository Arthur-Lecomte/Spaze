using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "WreckData", menuName = "ScriptableObjects/WreckData", order = 3)]
public class WreckData : ScriptableObject {
    public List<VariantData> variants;
}
