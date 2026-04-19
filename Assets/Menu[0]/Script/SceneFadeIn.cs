using UnityEngine;
using System.Collections;

// Este script debe ir en la escena del Laberinto, no en el Menú.
public class SceneFadeIn : MonoBehaviour
{
    // Arrastra aquí tu luz direccional (el sol) desde la Jerarquía
    public Light directionalLight;

    // La intensidad final que quieres que tenga la luz al terminar la transición
    public float finalLightIntensity = 1.0f; // Ajusta según tu gusto

    // Cuántos segundos quieres que dure la transición
    public float fadeDuration = 3.0f;

    // Almacenamos el color ambiental original para la transición
    private Color targetAmbientColor;

    private void Start()
    {
        // 1. Configuraciones iniciales (todo debe empezar oscuro)

        // Asignamos la luz direccional por defecto si no se ha asignado
        if (directionalLight == null)
            directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();

        if (directionalLight != null)
        {
            directionalLight.intensity = 0f; // Empezamos sin luz solar
        }

        // Si usas el modo 'Color' para la iluminación ambiental, también lo desvanecemos
        if (RenderSettings.ambientMode == UnityEngine.Rendering.AmbientMode.Flat)
        {
            targetAmbientColor = RenderSettings.ambientLight;
            RenderSettings.ambientLight = Color.black; // Empezamos con luz ambiental negra
        }

        // 2. Iniciamos la Corrutina de desvanecimiento
        StartCoroutine(PerformFadeIn());
    }

    IEnumerator PerformFadeIn()
    {
        // Esperamos un frame para evitar tirones en la carga
        yield return null;

        float currentTime = 0f;

        // Bucle que dura exactamente la 'fadeDuration'
        while (currentTime < fadeDuration)
        {
            // Incrementamos el tiempo transcurrido
            currentTime += Time.deltaTime;

            // Calculamos el progreso (un valor entre 0 y 1)
            float t = currentTime / fadeDuration;

            // 3. Aplicamos el incremento de luz (LERP)

            // Desvanecemos la intensidad de la luz direccional
            if (directionalLight != null)
            {
                directionalLight.intensity = Mathf.Lerp(0f, finalLightIntensity, t);
            }

            // Desvanecemos el color ambiental
            if (RenderSettings.ambientMode == UnityEngine.Rendering.AmbientMode.Flat)
            {
                RenderSettings.ambientLight = Color.Lerp(Color.black, targetAmbientColor, t);
            }

            // Esperamos al siguiente frame
            yield return null;
        }

        // 4. Aseguramos valores finales perfectos para evitar errores de redondeo
        if (directionalLight != null) directionalLight.intensity = finalLightIntensity;
        if (RenderSettings.ambientMode == UnityEngine.Rendering.AmbientMode.Flat) RenderSettings.ambientLight = targetAmbientColor;

        // Desactivamos el script para ahorrar rendimiento
        this.enabled = false;
    }
}