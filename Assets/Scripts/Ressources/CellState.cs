using System.Collections.Generic;
using UnityEngine;

public class CellState {
    public Vector3 Position { get; set; }
    public bool IsAsteroid { get; set; }
    public Ressource Ressource { get; set; }
    public bool IsMined { get; set; }
    public bool IsScavenged { get; set; }
}
