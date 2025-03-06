using System.Collections;
using System.Collections.Generic;
using Quantum;
using UnityEngine;

public class SpawnerEntityView : QuantumEntityViewComponent
{
    public override void OnActivate(Frame frame)
    {
        SpawnerController spawnerController = frame.Get<SpawnerController>(EntityRef);
        SpawnerSpec spawnerSpec = frame.FindAsset(spawnerController.SpawnerSpec);

        GetComponent<MeshRenderer>().material.color = spawnerSpec.Color;
    }
}
