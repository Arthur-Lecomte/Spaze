using System.Collections.Generic;
using UnityEngine;

public abstract class Module : MonoBehaviour {
    protected List<Module> Neighbors;
    private const float Scale = 2f;
    protected bool CanBeActivate;

    protected virtual void Awake() {
        Neighbors = new List<Module>();
        Vector3[] offsets = {
            new Vector3(1 * Scale, 0, 0), // à droite
            new Vector3(-1 * Scale, 0, 0), // à gauche
            new Vector3(0.5f * Scale, 0, 1 * Scale), // en haut à droite
            new Vector3(-0.5f * Scale, 0, 1 * Scale), // en haut à gauche
            new Vector3(0.5f * Scale, 0, -1 * Scale), // en bas à droite
            new Vector3(-0.5f * Scale, 0, -1 * Scale) // en bas à gauche
        };
        
        foreach (Vector3 offset in offsets) {
            Vector3 neighborPosition = transform.position + offset;

            Collider[] colliders = new Collider[1];
            Physics.OverlapSphereNonAlloc(neighborPosition, 0.1f, colliders, LayerMask.GetMask("Vaisseau"));
            foreach (Collider c in colliders) {
                if (c) {
                    Module module = c.GetComponent<Module>();
                    if (module) {
                        Neighbors.Add(module);
                        break;
                    }
                }
            }
        }
    }

    protected void CanActivateNeighbors() {
        foreach (Module module in Neighbors) {
            module.CanBeActivate = true;
        }
    }

    public virtual void ToggleBuildMode(bool value) {
    }
}
