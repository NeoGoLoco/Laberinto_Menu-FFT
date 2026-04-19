using UnityEngine;
using UnityEngine.EventSystems; // Para saber qué botón está seleccionado
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock; // Para acceder a los controles de PlayStation

public class ControlLucesMenu : MonoBehaviour
{
    [Header("Botones del Menú")]
    public GameObject botonJugar;
    public GameObject botonSalir;

    [Header("Colores del DualSense")]
    public Color colorJugar = Color.cyan;
    public Color colorSalir = Color.red;
    public Color colorPorDefecto = Color.white;

    void Update()
    {
        // --- EL SEGURO ANTI-MOUSE ---
        // Si Unity pierde la selección por un clic fantasma, lo forzamos a regresar al botón Jugar.
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(botonJugar);
        }
        // -----------------------------

        // Buscamos si hay un control de PS5 o PS4 conectado nativamente
        var dualSense = DualSenseGamepadHID.current;
        var dualShock = DualShockGamepad.current;

        // Si detecta un DualSense (PS5)
        if (dualSense != null)
        {
            ActualizarColorLED(dualSense);
        }
        // Fallback por si Unity 2019 lo detecta como DualShock (PS4)
        else if (dualShock != null)
        {
            ActualizarColorLED(dualShock);
        }
    }

    // Usamos la clase base DualShockGamepad que controla la luz de ambos
    void ActualizarColorLED(DualShockGamepad controlActivo)
    {
        // Le preguntamos a Unity: ¿En qué botón está parado el jugador ahorita?
        GameObject botonActual = EventSystem.current.currentSelectedGameObject;

        if (botonActual == botonJugar)
        {
            controlActivo.SetLightBarColor(colorJugar);
        }
        else if (botonActual == botonSalir)
        {
            controlActivo.SetLightBarColor(colorSalir);
        }
        else
        {
            // Por si haces clic fuera de los botones
            controlActivo.SetLightBarColor(colorPorDefecto);
        }
    }
}