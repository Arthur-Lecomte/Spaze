using UnityEngine;
using System.Collections.Generic;
namespace Spaze {
    [CreateAssetMenu(fileName = "WreckData", menuName = "ScriptableObjects/WreckData", order = 3)]
    public class WreckData : ScriptableObject {
        public List<VariantData> variants;
    }
}