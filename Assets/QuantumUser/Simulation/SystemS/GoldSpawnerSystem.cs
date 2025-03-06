namespace Quantum.PT
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class GoldSpawnerSystem : SystemMainThread, ISignalOnTriggerGoldWithCharacter
    {
        public unsafe void OnTriggerGoldWithCharacter(Frame f, EntityRef gold, Character* character)
        {
            if (f.Unsafe.TryGetPointerSingleton(out GoldSpawner* goldSpawner))
            {
                goldSpawner->CurrentGoldCount -= 1;
                f.Destroy(gold);
            }
        }

        public override void Update(Frame f)
        {
            if (f.Unsafe.TryGetPointerSingleton(out GoldSpawner* goldSpawner))
            {
                if (goldSpawner->CurrentGoldCount < goldSpawner->MaxGoldCount)
                {
                    EntityRef goldEntity = f.Create(goldSpawner->GoldPrototype);
                    Transform3D* goldTransform = f.Unsafe.GetPointer<Transform3D>(goldEntity);

                    int seed = HashCodeUtils.CombineHashCodes(f.RuntimeConfig.Seed, f.Number);
                    RNGSession sessionRNG = new RNGSession(seed);

                    FPVector3 targetPosition = new FPVector3(sessionRNG.Next(-10, 11), 0, sessionRNG.Next(-10, 11));

                    NavMesh navMesh = f.Map.NavMeshes["QuantumNavMesh"];

                    if (navMesh.FindClosestTriangle(f, targetPosition, 10, NavMeshRegionMask.Default, out int triangle, out FPVector3 closestPosition))
                    {
                        targetPosition = closestPosition;
                    }

                    goldTransform->Position = closestPosition;
                    goldSpawner->CurrentGoldCount += 1;
                }
            }
        }
    }
}
