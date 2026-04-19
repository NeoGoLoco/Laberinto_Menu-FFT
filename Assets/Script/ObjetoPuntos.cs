using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoPuntos : MonoBehaviour
{
    public GameObject ObjPuntos;
    public float puntosQueDa;
    public AudioClip elSonido;

    [Range(0f, 1f)]
    public float volumen = 0.3f;

    private bool yaRecogida = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !yaRecogida)
        {
            yaRecogida = true;

            if (ObjPuntos != null)
            {
                ObjPuntos.GetComponent<Puntos>().puntosPlayer += puntosQueDa;
            }

            if (elSonido != null)
            {
                // Reproducimos el sonido en la posición de la cámara (Main Camera)
                // para que se escuche siempre bien (como sonido 2D)
                if (Camera.main != null)
                {
                    AudioSource.PlayClipAtPoint(elSonido, Camera.main.transform.position, volumen);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(elSonido, transform.position, volumen);
                }
            }

            Destroy(gameObject);
        }
    }
}