using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puntos : MonoBehaviour
{
    // Start is called before the first frame update
    public float puntosPlayer;
    public Text TextoPuntos;

    // Update is called once per frame
    void Update()
    {
        TextoPuntos.text = "Puntos: " + puntosPlayer.ToString();
        
    }
}
