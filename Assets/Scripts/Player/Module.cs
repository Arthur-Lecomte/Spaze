using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Module : MonoBehaviour {
    protected List<Module> Neighbors;
    private const float Scale = 2f;
    protected bool CanBeActivate;
    
    protected Construction Construction;

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

    public virtual void DisplayModule(bool value) {
    }
    
    public List<SearchShield> GetSearchShieldAtDistance(Type type, int distance) {
        List<SearchShield> shields = new List<SearchShield>();
        List<Module> neighbors = GetNeighborsAtDistance(distance);
        Debug.Log(neighbors.Count); // DEBUG !!!
        foreach (Module neighbor in neighbors) {
            if (type.IsInstanceOfType(neighbor.Construction)) {
                shields.Add((SearchShield)neighbor.Construction);
            }
        }
        
        return shields;
    }
    
    private List<Module> GetNeighborsAtDistance(int distance) {
        if (distance < 1) return new List<Module>();
        if (distance == 1) return new List<Module>(Neighbors);

        HashSet<Module> visited = new HashSet<Module>(Neighbors);
        Queue<Module> queue = new Queue<Module>(Neighbors);

        for (int i = 1; i < distance; i++) {
            int levelSize = queue.Count;
            for (int j = 0; j < levelSize; j++) {
                Module current = queue.Dequeue();
                foreach (Module neighbor in current.Neighbors) {
                    if (visited.Add(neighbor)) {
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        return new List<Module>(visited);
    }
}
