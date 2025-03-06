using System.Collections;
using System.Collections.Generic;
using Quantum;
using UnityEngine;

public class GameContext : QuantumMonoBehaviour, IQuantumViewContext
{
    public CameraController CameraController;
    public List<CameraRef> CameraRefs;
}
