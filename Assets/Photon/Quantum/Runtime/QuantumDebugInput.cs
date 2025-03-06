namespace Quantum {
  using Photon.Deterministic;
  using UnityEngine;


  public class QuantumDebugInput : MonoBehaviour {

    private void OnEnable() {
      QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
    }

    public void PollInput(CallbackPollInput callback) {
      Quantum.Input i = new Quantum.Input();
      callback.SetInput(i, DeterministicInputFlags.Repeatable);
    }
  }
}
