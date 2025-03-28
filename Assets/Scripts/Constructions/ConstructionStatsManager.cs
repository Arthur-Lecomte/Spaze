using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
namespace Spaze {
    public class ConstructionStatsManager {
        private Dictionary<TypeConstruction, Dictionary<RarityConstruction, Dictionary<int, Dictionary<string, float>>>> Values =
            new Dictionary<TypeConstruction, Dictionary<RarityConstruction, Dictionary<int, Dictionary<string, float>>>>();

        /// <summary>
        /// Constructeur de la classe ConstructionStatsManager. Charge les statistiques des constructions à partir d'un fichier JSON.
        /// </summary>
        public ConstructionStatsManager() {
            TextAsset jsonFile = Resources.Load<TextAsset>("ConstructionStats");
            if (jsonFile != null) {
                Dictionary<string, Dictionary<string, StatData>> constructions = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, StatData>>>(jsonFile.text);

                foreach (var construction in constructions) {
                    TypeConstruction type = (TypeConstruction)Enum.Parse(typeof(TypeConstruction), construction.Key);
                    Values[type] = new Dictionary<RarityConstruction, Dictionary<int, Dictionary<string, float>>>();
                    for (int rarity = 0; rarity < 4; rarity++) {
                        Values[type][(RarityConstruction)rarity] = new Dictionary<int, Dictionary<string, float>>();
                        for (int level = 0; level < 5; level++) {
                            Values[type][(RarityConstruction)rarity][level] = new Dictionary<string, float>();
                            foreach (var variable in construction.Value) {
                                float value = variable.Value.rarityMultipliers[rarity] * (variable.Value.baseValue + variable.Value.levelValue * variable.Value.levelMultipliers[level]);
                                Values[type][(RarityConstruction)rarity][level][variable.Key] = value;
                            }
                        }
                    }
                }
            } else {
                Debug.LogError("Erreur lors du chargement du JSON file.");
            }
        }

        /// <summary>
        /// Obtient le dictionnaire des statistiques pour un type de construction, une rareté et un niveau spécifiés.
        /// </summary>
        /// <param name="type">Le type de construction.</param>
        /// <param name="rarity">La rareté de la construction.</param>
        /// <param name="level">Le niveau de la construction.</param>
        /// <returns>Un dictionnaire contenant les statistiques de la construction.</returns>
        public Dictionary<string, float> GetDico(TypeConstruction type, RarityConstruction rarity, int level) {
            if (Values.ContainsKey(type) && Values[type].ContainsKey(rarity) && Values[type][rarity].ContainsKey(level)) {
                return Values[type][rarity][level];
            }
            Debug.LogError("Erreur dans la récupération du dictionnaire pour " + type + " " + rarity + " " + level);
            return null;
        }

        [Serializable]
        private class StatData {
            public float baseValue;
            public float levelValue;
            public float[] levelMultipliers;
            public float[] rarityMultipliers;
        }
    }
}