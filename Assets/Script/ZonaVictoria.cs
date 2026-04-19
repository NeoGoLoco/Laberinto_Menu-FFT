using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZonaVictoria : MonoBehaviour
{
    public AudioClip sonidoVictoria;

    [Range(0f, 1f)]
    public float volumen = 1f; // Ajustado al máximo del rango

    // Tiempo de celebración antes de regresar al Menú
    public float tiempoParaRegresar = 2f;

    private bool yaGano = false;

    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si es el jugador
        if (other.tag == "Player" && !yaGano)
        {
            yaGano = true;

            // SONIDO DRAMÁTICO DE VICTORIA:
            if (sonidoVictoria != null && Camera.main != null)
            {
                AudioSource.PlayClipAtPoint(sonidoVictoria, Camera.main.transform.position, volumen);
            }

            // DETENER LA CÁMARA:
            // Le da un efecto cinemático muy padre que la cámara se quede congelada viendo al jugador
            if (Camera.main != null)
            {
                var scriptCamara = Camera.main.GetComponent<Camara>();
                if (scriptCamara != null)
                {
                    scriptCamara.enabled = false;
                }
            }

            // DESACTIVAR CONTROLES DEL JUGADOR:
            // Para que no se mueva mientras suena la música de victoria
            var scriptJugador = other.GetComponent<Jugador>();
            if (scriptJugador != null)
            {
                scriptJugador.enabled = false;
            }

            // REGRESAR AL MENÚ:
            // Iniciamos una cuenta atrás para volver al inicio
            Invoke("IrAlMenuPrincipal", tiempoParaRegresar);
        }
    }

    void IrAlMenuPrincipal()
    {
        // Carga la Escena 0 (Asegúrate de que tu Menú sea el índice 0 en Build Settings)
        SceneManager.LoadScene(0);
    }
}