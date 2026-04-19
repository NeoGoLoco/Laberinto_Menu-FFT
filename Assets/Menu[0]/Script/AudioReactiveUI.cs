using UnityEngine;
using UnityEngine.UI; 

public class AudioReactiveUI : MonoBehaviour
{
    [Header("Referencias")]
    public AudioSource audioSource;
    // "FondoVideo_Brillo"
    public RawImage shineImage;

    [Header("Ajustes de Oscilación")]
    // Sensibilidad del efecto (qué tanto reacciona al bajo)
    public float sensitivity = 50.0f;
    // La transparencia mínima y máxima que alcanzará que puede alcanzar el brillo
    public float minAlpha = 0.0f;
    public float maxAlpha = 0.8f;
    // Suavizado de la transición para la transición
    public float lerpSpeed = 15.0f;

    // Análisis de espectro de audio
    private float[] audioSamples = new float[512];
    private float targetAlpha;

    void Start()
    {
        if (audioSource == null) audioSource = FindObjectOfType<AudioSource>();
        if (shineImage == null) Debug.LogError("Por favor asigna la RawImage de brillo al script.");
    }

    void Update()
    {
        if (audioSource == null || shineImage == null) return;

        // 1. Analizar el espectro de audio (las frecuencias)
        audioSource.GetSpectrumData(audioSamples, 0, FFTWindow.Blackman);

        // 2. Nos enfocamos en las frecuencias BAJAS (bass/beat)
        // Usaremos las primeras 3 muestras (el bajo más potente)
        float currentBassLevel = 0f;
        int numSamplesToAverage = 4;
        for (int i = 0; i < numSamplesToAverage; i++)
        {
            currentBassLevel += audioSamples[i];
        }
        currentBassLevel /= numSamplesToAverage; // Promedio

        // 3. Convertimos el nivel del bajo en un valor de transparencia (Alpha)
        // Multiplicamos por la sensibilidad y limitamos entre min y max
        targetAlpha = Mathf.Clamp(currentBassLevel * sensitivity, minAlpha, maxAlpha);

        // 4. Suavizamos la transición (LERP) para que se sienta orgánica
        float currentAlpha = shineImage.color.a;
        float nextAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * lerpSpeed);

        // 5. Aplicamos el nuevo color a la RawImage
        Color finalColor = shineImage.color;
        finalColor.a = nextAlpha;
        shineImage.color = finalColor;
    }
}