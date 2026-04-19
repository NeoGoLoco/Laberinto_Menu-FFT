using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moneda : MonoBehaviour
{
    public enum Modo { HaciaArriba, HaciaAbajo }

    [Header("Configuración de Dirección")]
    [SerializeField] private Modo direccion = Modo.HaciaAbajo;

    [Header("Ajustes de Movimiento")]
    [SerializeField] private float velocidad = 1f;
    [SerializeField] private float distancia = -.5f;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Esta fórmula ajusta el Seno para que empiece en 0, suba a 1 y baje a 0
        // Evitando que el valor sea negativo.
        float movimientoControlado = (Mathf.Sin(Time.time * velocidad - Mathf.PI / 2) + 1f) / 2f;

        float desplazamiento = movimientoControlado * distancia;

        if (direccion == Modo.HaciaArriba)
        {
            // El objeto sube desde el suelo y vuelve
            transform.position = posicionInicial + new Vector3(0, desplazamiento, 0);
        }
        else
        {
            // El objeto baja desde el techo y vuelve
            transform.position = posicionInicial + new Vector3(0, -desplazamiento, 0);
        }
    }
}
