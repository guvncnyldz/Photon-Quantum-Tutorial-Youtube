using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using Quantum.Collections;
using Quantum.Physics3D;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.PT
{
    [Preserve]
    public unsafe class PlayerMovementSystem : SystemMainThreadFilter<PlayerMovementSystem.Filter>, ISignalOnTriggerGoldWithCharacter
    {
        NavMesh navMesh;
        public override void OnInit(Frame f)
        {
            navMesh = f.Map.GetNavMesh("QuantumNavMesh");

            base.OnInit(f);
        }

        public unsafe void OnTriggerGoldWithCharacter(Frame f, EntityRef gold, Character* character)
        {
            character->GoldCount += 1;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var input = f.GetPlayerInput(filter.Character->PlayerRef);

            if (input->LeftClick)
            {
                Hit3D? hit3D = f.Physics3D.Raycast(input->Origin, input->Direction, 100);

                if (hit3D.HasValue)
                {
                    var pathResult = f.Navigation.FindPath((FrameThreadSafe)f, filter.Transform3D->Position, hit3D.Value.Point, navMesh, NavMeshRegionMask.Default);

                    filter.MovementController->IsPathSucceeded = pathResult.Result == Navigation.FindPathResult.Code.Success;
                    filter.MovementController->Hit3D = (Hit3D)hit3D;
                }
            }

            if (input->LeftClick.WasReleased && f.IsVerified)
            {
                if (filter.MovementController->IsPathSucceeded)
                {
                    filter.PathFinder->SetTarget(f, filter.MovementController->Hit3D.Point, navMesh);

                    f.Events.OnPathCompletedEvent(f, filter.Character->PlayerRef);
                }
            }
        }

        public struct Filter
        {
            public EntityRef EntityRef;
            public Transform3D* Transform3D;
            public Character* Character;
            public NavMeshPathfinder* PathFinder;
            public MovementController* MovementController;
        }
    }
}
