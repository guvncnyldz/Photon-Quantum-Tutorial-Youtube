using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Deterministic;
using Quantum;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterEntityView : QuantumEntityViewComponent<GameContext>
{
    private bool _isLocalPlayer;
    private NavMesh navMesh;
    private InputAction _commandAction;
    [SerializeField] private MeshRenderer _model;
    public override void OnInitialize()
    {
        _commandAction = InputSystem.actions.FindAction("Command");
    }

    public override void OnActivate(Frame frame)
    {
        Character character = frame.Get<Character>(EntityRef);
        _isLocalPlayer = Game.PlayerIsLocal(character.PlayerRef);

        navMesh = frame.Map.GetNavMesh("QuantumNavMesh");

        SpawnerSpec spawnerSpec = frame.FindAsset(character.SpawnerSpec);
        _model.material.color = spawnerSpec.Color;

        if (_isLocalPlayer)
        {
            Vector3 cameraPosition = ViewContext.CameraRefs.FirstOrDefault(cameraRef => cameraRef.SpawerSpec.SpawnerID == spawnerSpec.SpawnerID).transform.position;
            ViewContext.CameraController.SetPosition(cameraPosition);
        }
    }

    void Update()
    {
        if (!_isLocalPlayer || !_commandAction.IsPressed())
            return;

        Ray ray = Camera.main.ScreenPointToRay(_commandAction.ReadValue<Vector2>());

        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            Frame frame = Game.Frames.Predicted;

            FPVector3 entityPosition = frame.Get<Transform3D>(EntityRef).Position;

            var PathResult = frame.Navigation.FindPath((FrameThreadSafe)frame, entityPosition, hit.point.ToFPVector3(), navMesh, NavMeshRegionMask.Default);

            if (PathResult.Result == Navigation.FindPathResult.Code.Success)
            {
                LineController.Instance.DrawLine(PathResult.PathSize, PathResult.Path);
            }
        }
    }
}
