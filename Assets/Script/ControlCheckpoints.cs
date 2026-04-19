using UnityEngine;

public class ControlCheckpoints : MonoBehaviour
{
    [HideInInspector] public Vector3 puntoDeReaparicion;

    void Start()
    {
        // Al empezar el nivel, su punto seguro es donde aparece por primera vez
        puntoDeReaparicion = transform.position;
    }
}