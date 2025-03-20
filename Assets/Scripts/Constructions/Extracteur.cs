using UnityEngine;
using System.Collections.Generic;

public class Extracteur : Construction {
    public static Extracteur Instance;
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private Transform extracteur;
    public float Speed => speed;
    public float Range => range;

    private Dictionary<int, VariantData> dictionaryVariant = new Dictionary<int, VariantData>();
    private Dictionary<int, int> dictionarySeed = new Dictionary<int, int>();
    private Dictionary<int, int> structureDictionaryData = new Dictionary<int, int>();
    int structureDictionaryInteration = 0;
    protected override void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    protected override void PerformUpgrade() {
        Debug.LogWarning(GetType() +" PerformUpgrade Not Implemented"); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }

    public override Dictionary<string, string> GetStats() {
        var stats = base.GetStats();
        stats.Add("Speed", speed.ToString("F2"));
        stats.Add("Range", range.ToString("F2"));
        return stats;
    }

    public void GetStructureVariantAndSeed(VariantData structureVariant, int structureSeed) {
        dictionaryVariant.Add(structureDictionaryInteration, structureVariant);
        dictionarySeed.Add(structureDictionaryInteration, structureSeed);
        structureDictionaryData.Add(structureDictionaryInteration, structureDictionaryInteration);
        structureDictionaryInteration++;

        foreach (var strucDicData in structureDictionaryData) {
            Debug.Log("VariantNumber: " + strucDicData.Key + " SeedNumber: " + strucDicData.Value);
            foreach(var dicVariant in dictionaryVariant) {
                if(strucDicData.Key == dicVariant.Key)
                {
                    Debug.Log("VariantKey: " + dicVariant.Key + " VariantValue: " + dicVariant.Value + " Seed: " + dictionarySeed[dicVariant.Key]);
                    break;
                }
            }
        }   
    }
}
