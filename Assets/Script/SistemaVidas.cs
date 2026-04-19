using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SistemaVidas : MonoBehaviour
{
    public static SistemaVidas Instance;

    [Header("Configuración de Vidas")]
    public int vidasMaximas = 7;
    public int vidasActuales;
    public float duracionFade = 2.0f;

    [Header("UI (Arrastra aquí tus elementos)")]
    public Text textoVidas;
    // Este objeto debe tener una imagen de fondo asignada en el Inspector
    public Image cortinaGameOver;

    [Header("Colores del Texto")]
    public Color colorLleno = Color.white;
    public Color colorPeligro = Color.red;

    private bool juegoTerminado = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        vidasActuales = vidasMaximas;
        ActualizarTextoVidas();

        if (cortinaGameOver != null)
        {
            // Obtenemos el color actual de la imagen 
            Color colorInicial = cortinaGameOver.color;
            // Forzamos el alpha a 0 para que empiece invisible, pero mantenemos su RGB
            colorInicial.a = 0f;
            cortinaGameOver.color = colorInicial;

            cortinaGameOver.raycastTarget = false;
        }
    }

    public void PerderVida()
    {
        if (juegoTerminado) return;

        vidasActuales--;
        ActualizarTextoVidas();

        if (vidasActuales <= 0)
        {
            juegoTerminado = true;
            StartCoroutine(SecuenciaGameOver());
        }
    }

    // Sumar Vidas
    public void SumarVidas(int cantidad)
    {
        if (juegoTerminado) return;

        vidasActuales += cantidad;
        //vidasActuales = Mathf.Min(vidasActuales, vidasMaximas); // Para que se limite el máximo
        ActualizarTextoVidas();
    }

    void ActualizarTextoVidas()
    {
        if (textoVidas != null)
        {
            textoVidas.text = "Vidas: " + Mathf.Max(0, vidasActuales).ToString();
            float porcentajeVidas = (float)vidasActuales / (float)vidasMaximas;
            textoVidas.color = Color.Lerp(colorPeligro, colorLleno, porcentajeVidas);
        }
    }

    IEnumerator SecuenciaGameOver()
    {
        float tiempoActual = 0f;
        if (cortinaGameOver != null) cortinaGameOver.raycastTarget = true;

        // Guardamos el color original de la imagen para no perderlo
        Color colorOriginalImagen = Color.white; // Valor por defecto seguro
        if (cortinaGameOver != null)
        {
            colorOriginalImagen = cortinaGameOver.color;
        }

        while (tiempoActual < duracionFade)
        {
            tiempoActual += Time.deltaTime;
            // Calculamos el alpha de 0 a 1
            float alpha = Mathf.Lerp(0f, 1f, tiempoActual / duracionFade);

            if (cortinaGameOver != null)
            {
                // Creamos un nuevo color basado en el original, pero con el alpha actualizado
                cortinaGameOver.color = new Color(colorOriginalImagen.r, colorOriginalImagen.g, colorOriginalImagen.b, alpha);
            }
            yield return null;
        }

        SceneManager.LoadScene(0);
    }
}