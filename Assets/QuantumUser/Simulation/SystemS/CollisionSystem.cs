namespace Quantum.PT
{
    using Photon.Deterministic;
    using Quantum;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class CollisionSystem : SystemSignalsOnly, ISignalOnTriggerEnter3D
    {
        public void OnTriggerEnter3D(Frame f, TriggerInfo3D info)
        {
            if (f.TryGet(info.Entity, out Gold gold))
            {
                if(f.Unsafe.TryGetPointer(info.Other, out Character* character))
                {
                    f.Signals.OnTriggerGoldWithCharacter(info.Entity, character);
                }
            }
        }
    }
}
