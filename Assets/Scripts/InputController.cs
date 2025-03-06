using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using Quantum;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private InputAction _commandAction;

    private void OnEnable()
    {
        QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
        _commandAction = InputSystem.actions.FindAction("Command");
    }

    public void PollInput(CallbackPollInput callback)
    {
        Quantum.Input i = new Quantum.Input();

        i.LeftClick = _commandAction.IsPressed();
 
        Ray ray = Camera.main.ScreenPointToRay(_commandAction.ReadValue<Vector2>());

        i.Direction = ray.direction.ToFPVector3();
        i.Origin = ray.origin.ToFPVector3();

        callback.SetInput(i, DeterministicInputFlags.Repeatable);
    }
}
