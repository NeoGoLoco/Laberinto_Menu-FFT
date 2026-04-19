using UnityEngine;

// Aseguramos que el objeto tenga un Renderer para poder acceder al material
[RequireComponent(typeof(Renderer))]
public class MusicReactiveEmission : MonoBehaviour
{
    // Enumeración para elegir qué frecuencia debe escuchar este objeto
    public enum TipoFrecuencia { Bajos, Medios, Agudos }

    [Header("Configuración del Brillo")]
    // ¿A qué parte de la canción reacciona este objeto?
    public TipoFrecuencia frecuenciaAHeard = TipoFrecuencia.Bajos;

    // Color de brillo máximo (cuando la música golpea fuerte)
    [ColorUsage(true, true)] // Habilita el selector de color HDR para brillo intenso
    public Color colorEmision = Color.cyan;

    // Qué tan rápido suaviza el brillo (para que no parpadee feo)
    public float velocidadSuavizado = 15f;

    // Variables internas
    private Renderer miRenderer;
    private Material miMaterial;
    private Color colorActualEmission;

    void Start()
    {
        miRenderer = GetComponent<Renderer>();
        // Importante: miRenderer.material genera una copia única del material
        // para este objeto, así no brillan todos los objetos iguales de la escena a la vez.
        miMaterial = miRenderer.material;

        // Aseguramos que el Material tenga la casilla "Emission" activada
        miMaterial.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        // Si el Cerebro Musical no existe, no hacemos nada
        if (MusicDataManager.Instance == null) return;

        // Obtenemos el valor de la frecuencia asignada desde el Cerebro
        float intensidadMusica = 0f;

        switch (frecuenciaAHeard)
        {
            case TipoFrecuencia.Bajos:
                intensidadMusica = MusicDataManager.Instance.ValorBajo;
                break;
            case TipoFrecuencia.Medios:
                intensidadMusica = MusicDataManager.Instance.ValorMedio;
                break;
            case TipoFrecuencia.Agudos:
                intensidadMusica = MusicDataManager.Instance.ValorAgudo;
                break;
        }

        // MATEMÁTICA LUMINOSA:
        // Mezclamos el color base (Negro/Apagado) con nuestro color de emisión según la intensidad
        Color colorDestino = Color.Lerp(Color.black, colorEmision, intensidadMusica);

        // SUAVIZADO: Transición suave de color
        colorActualEmission = Color.Lerp(colorActualEmission, colorDestino, Time.deltaTime * velocidadSuavizado);

        // APLICAR AL MATERIAL 3D:
        // Usamos la propiedad estándar "_EmissionColor" para encender el brillo
        miMaterial.SetColor("_EmissionColor", colorActualEmission);
    }
}