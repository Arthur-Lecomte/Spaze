using UnityEngine;
namespace Spaze {
    [CreateAssetMenu(fileName = "VariantData", menuName = "ScriptableObjects/VariantData", order = 1)]
    public class VariantData : ScriptableObject {
        public GameObject prefab;
        public TypeRessource ressourceType;
        public float probability;
    }
}