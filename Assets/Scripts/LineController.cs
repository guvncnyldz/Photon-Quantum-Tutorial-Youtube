using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Deterministic;
using Quantum;
using Quantum.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineController : MonoBehaviour
{
    public static LineController Instance;
    [SerializeField] private LineRenderer lineRenderer;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        QuantumEvent.Subscribe(this, (EventOnPathCompletedEvent e) => ResetLine(e.F, e.PlayerRef));
    }

    public void DrawLine(int pathSize, Navigation.PathVertex[] vertices)
    {
        Vector3[] viewPath = vertices.Map(vertex => vertex.Point.ToUnityVector3());

        lineRenderer.positionCount = pathSize;
        lineRenderer.SetPositions(viewPath);
    }

    void ResetLine(Frame f, PlayerRef playerRef)
    {
        if (!QuantumRunner.DefaultGame.PlayerIsLocal(playerRef))
            return;

        lineRenderer.positionCount = 0;
    }
}
