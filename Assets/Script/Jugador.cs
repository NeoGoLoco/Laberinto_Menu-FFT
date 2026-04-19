using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class Jugador : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 10f;
    public float maxSpeed = 5f;

    public float normalJumpingForce = 5f;
    public float extraJumpingForce = 12f;
    private float currentJumpingForce;

    public float distanciaRayoSuelo = 1.1f;

    private bool jumpRequest = false;
    public GameObject referencia;
    AudioSource audioSource;

    private bool estabaEnSuelo = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        currentJumpingForce = normalJumpingForce;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        bool presionoSalto = false;
        var gamepad = Gamepad.current;

        // Leemos la X de PlayStation
        if (gamepad != null && gamepad.buttonSouth.wasPressedThisFrame)
        {
            presionoSalto = true;
        }
        else if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            presionoSalto = true;
        }

        // Guardamos el estado actual del suelo en este fotograma exacto
        bool enSueloAhora = EstaEnSuelo();

        // 1. Lógica para saltar
        if (presionoSalto && enSueloAhora)
        {
            jumpRequest = true;
        }

        // LÓGICA DE ATERRIZAJE (IMPACTO) 
        // Si antes estaba en el aire (false) y ahora está en el suelo (true) = ¡Choque!
        if (!estabaEnSuelo && enSueloAhora)
        {
            // Iniciamos la vibración (Duración: 0.15s, MotorGrave: 0.1f, MotorAgudo: 0.4f)
            // Ajusta estos valores para que se sienta como un "golpecito" y no un terremoto
            StartCoroutine(VibrarControl(0.15f, 0.1f, 0.4f));
        }

        // Actualizamos la memoria para el siguiente fotograma
        estabaEnSuelo = enSueloAhora;
    }

    void FixedUpdate()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 velocidadHorizontal = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (velocidadHorizontal.magnitude > maxSpeed)
        {
            Vector3 velocidadLimitada = velocidadHorizontal.normalized * maxSpeed;
            rb.velocity = new Vector3(velocidadLimitada.x, rb.velocity.y, velocidadLimitada.z);
        }

        rb.AddForce(moveVertical * referencia.transform.forward * speed);
        rb.AddForce(moveHorizontal * referencia.transform.right * speed);

        // Tu salto se queda igualito
        if (jumpRequest)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * currentJumpingForce, ForceMode.Impulse);

            if (audioSource != null) audioSource.Play();
            jumpRequest = false;
        }
    }

    bool EstaEnSuelo()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, distanciaRayoSuelo))
        {
            if (hit.collider.CompareTag("MasSalto"))
            {
                currentJumpingForce = extraJumpingForce;
            }
            else
            {
                currentJumpingForce = normalJumpingForce;
            }
            return true;
        }
        return false;
    }

    // CORRUTINA DE VIBRACIÓN 
    IEnumerator VibrarControl(float duracion, float motorBajo, float motorAlto)
    {
        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            // Encendemos motores
            gamepad.SetMotorSpeeds(motorBajo, motorAlto);

            // Esperamos la fracción de segundo que dura el impacto
            yield return new WaitForSeconds(duracion);

            // Apagamos los motores...
            gamepad.SetMotorSpeeds(0f, 0f);
        }
    }

    // Si se cierra el juego, pausa, o destruye al jugador mientras vibra,
    // esta función nativa de Unity apaga el control para que no se vuelva loco.
    void OnDisable()
    {
        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0f, 0f);
        }
    }
}