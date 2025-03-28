using UnityEngine;
using System.Collections.Generic;
namespace Spaze {
    [CreateAssetMenu(fileName = "AsteroidData", menuName = "ScriptableObjects/AsteroidData", order = 2)]
    public class AsteroidData : ScriptableObject {
        public List<VariantData> variants;
    }
}