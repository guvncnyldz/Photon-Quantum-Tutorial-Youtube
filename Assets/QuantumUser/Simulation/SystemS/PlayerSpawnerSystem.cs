using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.PT
{
    [Preserve]
    public unsafe class PlayerSpawnerSystem : SystemSignalsOnly, ISignalOnPlayerAdded, ISignalOnPlayerRemoved
    {
        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
            foreach (var (spawnerEntity, spawnerComponent) in f.Unsafe.GetComponentBlockIterator<SpawnerController>())
            {
                if (spawnerComponent->PlayerRef != -1)
                    continue;

                spawnerComponent->PlayerRef = player;

                var assetRef = f.GetPlayerData(player).PlayerAvatar;
                var entity = f.Create(assetRef);

                if (f.Unsafe.TryGetPointer(entity, out Character* character))
                {
                    character->PlayerRef = player;
                    character->Health = 50;
                    character->SpawnerSpec = spawnerComponent->SpawnerSpec;
                }

                if (f.Unsafe.TryGetPointer(entity, out Transform3D* transform3D))
                {
                    NavMesh navMesh = f.Map.GetNavMesh("QuantumNavMesh");

                    FPVector3 spawnPosition = f.Get<Transform3D>(spawnerEntity).Position;

                    if (navMesh.FindClosestTriangle(f, spawnPosition, 10, NavMeshRegionMask.Default, out int triangle, out FPVector3 closestPosition))
                    {
                        spawnPosition = closestPosition;
                    }

                    transform3D->Position = spawnPosition;
                    transform3D->Rotation = FPQuaternion.LookRotation(FPVector3.Zero - spawnPosition);
                }

                break;
            }
        }

        public void OnPlayerRemoved(Frame f, PlayerRef player)
        {
            foreach (var character in f.GetComponentIterator<Character>())
            {
                if (character.Component.PlayerRef == player)
                {
                    f.Destroy(character.Entity);
                    break;
                }
            }

            foreach (var spawner in f.Unsafe.GetComponentBlockIterator<SpawnerController>())
            {
                if (spawner.Component->PlayerRef == player)
                {
                    spawner.Component->PlayerRef = -1;
                    break;
                }
            }
        }
    }
}
