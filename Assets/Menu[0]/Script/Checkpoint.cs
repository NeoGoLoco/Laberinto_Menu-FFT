using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Efectos del Checkpoint")]
    public AudioClip sonidoCheckpoint;

    [Header("Recompensas")]

    public int vidasDeRegalo = 2;

    private AudioSource audioSource;
    private bool yaActivado = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && sonidoCheckpoint != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f;
            audioSource.playOnAwake = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !yaActivado)
        {
            ControlCheckpoints control = other.GetComponent<ControlCheckpoints>();
            if (control != null)
            {
                control.puntoDeReaparicion = transform.position + new Vector3(0, 1f, 0);

                if (sonidoCheckpoint != null && audioSource != null)
                {
                    audioSource.PlayOneShot(sonidoCheckpoint);
                }


                if (SistemaVidas.Instance != null)
                {
                    SistemaVidas.Instance.SumarVidas(vidasDeRegalo);
                }

                yaActivado = true;
            }
        }
    }
}