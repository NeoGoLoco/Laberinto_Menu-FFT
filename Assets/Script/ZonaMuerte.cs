using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaMuerte : MonoBehaviour
{
    public AudioClip sonidoMuerte;
    [Range(0f, 1f)]
    public float volumen = 1f;

    public float tiempoParaReiniciar = 2f;
    private bool yaSeMurio = false;

    private GameObject jugadorQueCayo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !yaSeMurio)
        {
            yaSeMurio = true;
            jugadorQueCayo = other.gameObject;

            // RESTAR VIDA AL SISTEMA: 
            if (SistemaVidas.Instance != null)
            {
                SistemaVidas.Instance.PerderVida();
            }

            // SONIDO DRAMÁTICO:
            if (sonidoMuerte != null && Camera.main != null)
            {
                AudioSource.PlayClipAtPoint(sonidoMuerte, Camera.main.transform.position, volumen);
            }

            // DETENER LA CÁMARA:
            if (Camera.main != null)
            {
                var scriptCamara = Camera.main.GetComponent<Camara>();
                if (scriptCamara != null)
                {
                    scriptCamara.enabled = false;
                }
            }

            // DESACTIVAR CONTROLES DEL JUGADOR:
            var scriptJugador = other.GetComponent<Jugador>();
            if (scriptJugador != null)
            {
                scriptJugador.enabled = false;
            }

            // DECIDIR SI REAPARECE O ES GAME OVER:
            // Solo lo teletransportamos de regreso si le queda al menos 1 vida.
            // Si no hay SistemaVidas en la escena (para pruebas), lo reaparece por defecto.
            if (SistemaVidas.Instance == null || SistemaVidas.Instance.vidasActuales > 0)
            {
                Invoke("ReaparecerJugador", tiempoParaReiniciar);
            }
        }
    }

    void ReaparecerJugador()
    {
        // Buscamos el script de checkpoints en el jugador
        ControlCheckpoints control = jugadorQueCayo.GetComponent<ControlCheckpoints>();
        if (control != null)
        {
            // Lo teletransportamos al último checkpoint tocado
            jugadorQueCayo.transform.position = control.puntoDeReaparicion;
        }

        // Frenamos la caída para que no reaparezca acumulando velocidad
        Rigidbody rb = jugadorQueCayo.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }

        // VOLVER A ACTIVAR LOS CONTROLES Y LA CÁMARA
        var scriptJugador = jugadorQueCayo.GetComponent<Jugador>();
        if (scriptJugador != null)
        {
            scriptJugador.enabled = true;
        }

        if (Camera.main != null)
        {
            var scriptCamara = Camera.main.GetComponent<Camara>();
            if (scriptCamara != null)
            {
                scriptCamara.enabled = true;
            }
        }

        // Reiniciamos la variable para que pueda volver a morir en este precipicio
        yaSeMurio = false;
    }
}