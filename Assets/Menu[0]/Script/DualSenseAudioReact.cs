using UnityEngine;
using UnityEngine.InputSystem.DualShock; // La librería mágica de PlayStation

public class DualSenseAudioReact : MonoBehaviour
{
    // Creamos la lista para elegir qué parte de la canción queremos escuchar
    public enum RangoFrecuencia { Bajos, Medios, Agudos }

    [Header("Referencias")]
    public AudioSource musicaBase;

    [Header("Configuración del Ritmo")]
    // ¡AQUÍ ELIGES QUÉ ESCUCHA EL CONTROL!
    public RangoFrecuencia frecuenciaAEscuchar = RangoFrecuencia.Medios;

    // OJO: Los medios suelen necesitar MUCHA más sensibilidad que los bajos.
    // Si lo pones en medios y casi no brilla, sube esto a 100, 200 o más.
    public float sensibilidad = 80f;
    public float velocidadSuavizado = 15f;

    [Header("Colores (Base vs Golpe)")]
    public Color colorTranquilo = Color.black;
    public Color colorGolpe = Color.cyan; // Lo cambié a cyan para que combine con tus medios, ¡cámbialo a gusto!

    // Variables internas
    private float[] samplesAudio = new float[512];
    private Color colorActualLED;

    void Start()
    {
        if (musicaBase == null) musicaBase = GetComponent<AudioSource>();
        colorActualLED = colorTranquilo;
    }

    void Update()
    {
        var dualSense = DualSenseGamepadHID.current;

        if (dualSense == null || musicaBase == null || !musicaBase.isPlaying) return;

        musicaBase.GetSpectrumData(samplesAudio, 0, FFTWindow.Blackman);

        float promedioEnergia = 0f;

        // Cazamos la frecuencia dependiendo de lo que hayas elegido en el Inspector
        switch (frecuenciaAEscuchar)
        {
            case RangoFrecuencia.Bajos:
                // Índices 0 al 10 (El bombo pesado)
                for (int i = 0; i < 10; i++) promedioEnergia += samplesAudio[i];
                promedioEnergia /= 10f;
                break;

            case RangoFrecuencia.Medios:
                // Índices 10 al 250 (El cuerpo de la canción, melodías)
                for (int i = 10; i < 250; i++) promedioEnergia += samplesAudio[i];
                promedioEnergia /= 240f;
                break;

            case RangoFrecuencia.Agudos:
                // Índices 250 al 511 (Platillos, silbidos)
                for (int i = 250; i < 512; i++) promedioEnergia += samplesAudio[i];
                promedioEnergia /= 262f;
                break;
        }

        // Convertimos la energía en una intensidad de 0 a 1
        float intensidad = Mathf.Clamp01(promedioEnergia * sensibilidad);

        // Mezclamos los colores
        Color colorDestino = Color.Lerp(colorTranquilo, colorGolpe, intensidad);
        colorActualLED = Color.Lerp(colorActualLED, colorDestino, Time.deltaTime * velocidadSuavizado);

        // ¡Iluminamos el control físico!
        dualSense.SetLightBarColor(colorActualLED);
    }
}